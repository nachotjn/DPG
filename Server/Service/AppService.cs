using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class AppService : IAppService{
    private readonly IAppRepository appRepository;
    private readonly UserManager<Player> userManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AppService(
        IAppRepository appRepository, 
        UserManager<Player> userManager, 
        IHttpContextAccessor httpContextAccessor){
        this.appRepository = appRepository;
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
    }
    //Players
    public async Task<PlayerDto> CreatePlayer(CreatePlayerDto createPlayerDto){
        // This is to get the user
        var currentUser = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);

        // Thi is to check if the user is an admin, and throw and exception if not
        if (!(await userManager.IsInRoleAsync(currentUser, "Admin"))){
            throw new UnauthorizedAccessException("Only admins can create players.");
        }
        var validationContext = new ValidationContext(createPlayerDto);
        Validator.ValidateObject(createPlayerDto, validationContext, validateAllProperties: true);

        var player = createPlayerDto.ToPlayer();
        var newPlayer = await appRepository.CreatePlayer(player, createPlayerDto.Password);
        var result = await userManager.AddToRoleAsync(newPlayer, "Player");
         if (!result.Succeeded){
            throw new Exception("Failed to assign the default role 'Player' to the new player.");
        }
        
        return new PlayerDto().FromEntity(newPlayer);
    }

    public List<Player> GetAllPlayers(){
        return appRepository.GetAllPlayers().ToList();
    }

    public void UpdatePlayer(PlayerDto playerDto){
        var validationContext = new ValidationContext(playerDto);
        Validator.ValidateObject(playerDto, validationContext, validateAllProperties: true);

        var existingPlayer = appRepository.GetPlayerById(playerDto.PlayerId);
        if (existingPlayer == null){
            throw new Exception($"Player with ID {playerDto.PlayerId} not found.");
        }

        existingPlayer.UserName = playerDto.Name ?? existingPlayer.UserName;
        existingPlayer.Email = playerDto.Email ?? existingPlayer.Email;
        existingPlayer.PhoneNumber = playerDto.Phone ?? existingPlayer.PhoneNumber;
        //existingPlayer.Isadmin = playerDto.IsAdmin;
        existingPlayer.Isactive = playerDto.IsActive;
        existingPlayer.Balance = playerDto.Balance;

        if (!playerDto.Updatedat.HasValue){
            playerDto.Updatedat = DateTime.UtcNow; 
        }
        existingPlayer.Updatedat = playerDto.ToDatabaseKind(playerDto.Updatedat);

        appRepository.UpdatePlayer(existingPlayer);
    }

    public List<PlayerDto> GetPlayersForGame(Guid gameId){
        var players = appRepository.GetPlayersForGame(gameId);
        return players.Select(player => new PlayerDto().FromEntity(player)).ToList();
    }

    

    //Boards
    public BoardDto CreateBoard(CreateBoardDto createBoardDto){
        var validationContext = new ValidationContext(createBoardDto);
        Validator.ValidateObject(createBoardDto, validationContext, validateAllProperties: true);

        var player = appRepository.GetPlayerById(createBoardDto.Playerid);
        var game = appRepository.GetGameById(createBoardDto.Gameid);

        if (player == null)
        throw new ArgumentException($"Player with ID {createBoardDto.Playerid} does not exist.");
        if (game == null)
        throw new ArgumentException($"Game with ID {createBoardDto.Gameid} does not exist.");
        if (!player.Isactive)
        throw new ArgumentException($"Player with ID {createBoardDto.Playerid} is not active");
        if(game.Iscomplete)
        throw new ArgumentException($"Game with ID {createBoardDto.Gameid} is complete, cant play");


        

        var board = createBoardDto.ToBoard();
        board.Player = player;
        board.Game = game;

         int boardCost = board.Numbers.Count switch{
            5 => 20,
            6 => 40,
            7 => 80,
            8 => 160,
            _ => throw new ArgumentException("Invalid number of fields. Boards can only have 5 to 8 fields.")
        };

        int totalCost = boardCost;
        int autoplayWeeks = board.Isautoplay ? (board.Autoplayweeks ?? 0) : 0;

        if (autoplayWeeks > 0){
            totalCost += boardCost * autoplayWeeks;
        }

        if (player.Balance < totalCost){
        throw new InvalidOperationException($"Player with ID {player.Id} does not have enough balance to create this board(s)");
        }
        player.Balance -= totalCost;
        appRepository.UpdatePlayer(player);

        game.Prizesum += boardCost * 0.7m;
        appRepository.UpdateGame(game);
        var createdBoard = appRepository.CreateBoard(board);

        
        if (autoplayWeeks > 0){
            CreateAutoplayBoards(board, player, game, boardCost, autoplayWeeks);
        }

        return new BoardDto().FromEntity(createdBoard);
    }

    private void CreateAutoplayBoards(Board board, Player player, Game startingGame, int boardCost, int autoplayWeeks){
        int currentWeek = startingGame.Weeknumber;
        int currentYear = startingGame.Year;

        for (int i = 1; i <= autoplayWeeks; i++){   
            currentWeek++;
            if (currentWeek > 52){
                currentWeek = 1;
                currentYear++;
            }

            
            var existingGame = appRepository.GetGameByWeekAndYear(currentWeek, currentYear);
            if (existingGame == null){
                existingGame = new Game{
                    Weeknumber = currentWeek,
                    Year = currentYear,
                    Prizesum = 0,
                    Iscomplete = false,
                    Createdat = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified)
                };

                appRepository.CreateGame(existingGame);
            }

            
            var autoplayBoard = new Board{
                Playerid = player.Id,
                Gameid = existingGame.Gameid,
                Numbers = board.Numbers,
                Isautoplay = false, 
                Autoplayweeks = null 
            };

            existingGame.Prizesum += boardCost * 0.7m;
            appRepository.UpdateGame(existingGame);
            appRepository.CreateBoard(autoplayBoard);
        }
    }


    public List<Board> GetAllBoards()
    {
        return appRepository.GetAllBoards().ToList();
    }

    public List <BoardDto> GetBoardsForPlayer(Guid playerId){
        var boards = appRepository.GetBoardsForPlayer(playerId);
        return boards.Select(board => new BoardDto().FromEntity(board)).ToList();
    }

    public List <BoardDto> GetBoardsForGame(Guid gameId){
        var boards = appRepository.GetBoardsForGame(gameId);
        return boards.Select(board => new BoardDto().FromEntity(board)).ToList();
    }



    //Games
    public GameDto CreateGame(CreateGameDto createGameDto){
        var validationContext = new ValidationContext(createGameDto);
        Validator.ValidateObject(createGameDto, validationContext, validateAllProperties: true);

        var existingGame = appRepository.GetGameByWeekAndYear(createGameDto.Weeknumber, createGameDto.Year);
        if(existingGame != null){
            throw new InvalidOperationException($"A game already exists for week {createGameDto.Weeknumber} of {createGameDto.Year}.");
        }
        var game = createGameDto.ToGame();
        Game newGame = appRepository.CreateGame(game);
        return new GameDto().FromEntity(newGame);
    }

    public List<Game> GetAllGames()
    {
        return appRepository.GetAllGames().ToList();
    }

    public void UpdateGame(GameDto gameDto){
        var validationContext = new ValidationContext(gameDto);
        Validator.ValidateObject(gameDto, validationContext, validateAllProperties: true);

        var existingGame = appRepository.GetGameById(gameDto.GameID);
        if (existingGame == null){
            throw new Exception($"Game with ID {gameDto.GameID} not found.");
        }

        existingGame.Iscomplete = gameDto.Iscomplete;
        existingGame.Winningnumbers = gameDto.Winningnumbers;
        existingGame.Prizesum = gameDto.Prizesum;
        

        if (!gameDto.Updatedat.HasValue){
            gameDto.Updatedat = DateTime.UtcNow; 
        }
        existingGame.Updatedat = gameDto.ToDatabaseKind(gameDto.Updatedat);

        appRepository.UpdateGame(existingGame);
    }

    //Winners
    public WinnerDto CreateWinner(CreateWinnerDto createWinnerDto){
        var player = appRepository.GetPlayerById(createWinnerDto.Playerid);
        var game = appRepository.GetGameById(createWinnerDto.Gameid);
        var board = appRepository.GetBoardByID(createWinnerDto.Boardid);

        if (player == null)
        throw new ArgumentException($"Player with ID {createWinnerDto.Playerid} does not exist.");
        if (game == null)
        throw new ArgumentException($"Game with ID {createWinnerDto.Gameid} does not exist.");
        if (board == null)
        throw new ArgumentException($"Board with ID {createWinnerDto.Boardid} does not exist.");
    
        var winner = createWinnerDto.ToWinner();
        winner.Player = player;
        winner.Game = game;
        winner.Board = board;
        Winner newWinner = appRepository.CreateWinner(winner);
        return new WinnerDto().FromEntity(winner);
    }

    public List<Winner> GetAllWinners(){
        return appRepository.GetAllWinners().ToList();
    }

    public List <WinnerDto> GetWinnersForGame(Guid gameId){
        var winners = appRepository.GetWinnersForGame(gameId);
        return winners.Select(winner => new WinnerDto().FromEntity(winner)).ToList();
    }

    public void DetermineWinnersForGame(Guid gameId){
        var game = appRepository.GetGameById(gameId);
        if (game == null){
            throw new Exception($"Game with ID {gameId} not found.");
        }
        if (!game.Iscomplete){
            throw new InvalidOperationException($"Game with ID {gameId} is not complete");
        }
        if(game.Winningnumbers == null){
            throw new Exception($"Game with ID {gameId} doenst have a winning sequence");
        }

        var boards = appRepository.GetBoardsForGame(gameId);

        
        var winningBoards = boards.Where(board =>
            game.Winningnumbers.All(number => board.Numbers.Contains(number))).ToList();

        if (!winningBoards.Any()){
            CarryOverPrizeSum(game);
            return;
        }

        
        var winningPlayers = winningBoards
            .GroupBy(board => board.Playerid)
            .ToList();

        int totalShares = winningPlayers.Count;
        decimal prizePerShare = Math.Min(5000m, (game.Prizesum ?? 0m) / totalShares);

        decimal totalPrizeDistributed = 0;

        foreach (var group in winningPlayers){
            var player = group.First().Player; 
            var representativeBoard = group.First(); // This is a random winning board for the player, have to change the db

            var winner = new Winner{
                Gameid = game.Gameid,
                Boardid = representativeBoard.Boardid, 
                Playerid = player.Id,
                Game = game,
                Board = representativeBoard,
                Player = player,
                Winningamount = prizePerShare
            };

            appRepository.CreateWinner(winner);
            totalPrizeDistributed += prizePerShare;
        }

        
        decimal? surplus = game.Prizesum - totalPrizeDistributed;
        if (surplus > 0){
            CarryOverPrizeSum(game, surplus);
        }
    }


   private void CarryOverPrizeSum(Game game, decimal? surplus = 0){
    var nextGame = appRepository.GetNextGame(game);
        if (nextGame == null){
            int nextWeek = game.Weeknumber + 1;
            int nextYear = game.Year;

            if (nextWeek > 52) {
                nextWeek = 1;
                nextYear += 1;
            }

            nextGame = new Game{
                Weeknumber = nextWeek,
                Year = nextYear,
                Prizesum = surplus > 0 ? surplus : game.Prizesum,
                Iscomplete = false, 
                Createdat = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified)
            };

            appRepository.CreateGame(nextGame);
        }
        else{
            nextGame.Prizesum += surplus > 0 ? surplus : game.Prizesum;
            appRepository.UpdateGame(nextGame);
        }
    }


    
    public void UpdateWinner(WinnerDto winnerDto, decimal winningAmount){
        var existingWinner = appRepository.GetWinnerById(winnerDto.Winnerid);
        if (existingWinner == null){
            throw new Exception($"Winner with ID {winnerDto.Winnerid} not found.");
        }

        existingWinner.Winningamount = winningAmount;
       
        appRepository.UpdateWinner(existingWinner);
    }


    //Transactions
    public TransactionDto CreateTransaction(CreateTransactionDto createTransactionDto){
        var player = appRepository.GetPlayerById(createTransactionDto.Playerid);
        if (player == null)
        throw new ArgumentException($"Player with ID {createTransactionDto.Playerid} does not exist.");

        var transaction = createTransactionDto.ToTransaction();
        transaction.Player = player;
        Transaction newTransaction = appRepository.CreateTransaction(transaction);
        return new TransactionDto().FromEntity(transaction);
    }

    public List<TransactionDto> GetTransactionsForPlayer(Guid playerId){
        var transactions = appRepository.GetTransactionsForPlayer(playerId);
        return transactions.Select(transaction => new TransactionDto().FromEntity(transaction)).ToList();
    }

    public void UpdateTransaction(TransactionDto transactionDto, bool isconfirmed){
        var existingTransaction = appRepository.GetTransactionById(transactionDto.Transactionid);
        if (existingTransaction == null){
            throw new Exception($"Transaction with ID {transactionDto.Transactionid} not found.");
        }

        if (!existingTransaction.Isconfirmed && isconfirmed){
            var player = appRepository.GetPlayerById(existingTransaction.Playerid);
            if (player == null){
                throw new Exception($"Player with ID {existingTransaction.Playerid} not found.");
            }

            player.Balance += existingTransaction.Amount; 
            appRepository.UpdatePlayer(player); 
        }

        existingTransaction.Isconfirmed = isconfirmed;

        appRepository.UpdateTransaction(existingTransaction);
    }


    
}
