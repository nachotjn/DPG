using DataAccess.Models;

namespace Service;



public class AppService(IAppRepository appRepository) : IAppService{
    //Players
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto){
        var player = createPlayerDto.ToPlayer();
        Player newPlayer = appRepository.CreatePlayer(player);
        return new PlayerDto().FromEntity(newPlayer);
    }

    public List<Player> GetAllPlayers(){
        return appRepository.GetAllPlayers().ToList();
    }

    public void UpdatePlayer(PlayerDto playerDto){
        var existingPlayer = appRepository.GetPlayerById(playerDto.PlayerId);
        if (existingPlayer == null){
            throw new Exception($"Player with ID {playerDto.PlayerId} not found.");
        }

        existingPlayer.Name = playerDto.Name ?? existingPlayer.Name;
        existingPlayer.Email = playerDto.Email ?? existingPlayer.Email;
        existingPlayer.Phone = playerDto.Phone ?? existingPlayer.Phone;
        existingPlayer.Isadmin = playerDto.IsAdmin;
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
        var player = appRepository.GetPlayerById(createBoardDto.Playerid);
        var game = appRepository.GetGameById(createBoardDto.Gameid);

        if (player == null)
        throw new ArgumentException($"Player with ID {createBoardDto.Playerid} does not exist.");
        if (game == null)
        throw new ArgumentException($"Game with ID {createBoardDto.Gameid} does not exist.");

        var board = createBoardDto.ToBoard();
        board.Player = player;
        board.Game = game;
        var newBoard = appRepository.CreateBoard(board);
        return new BoardDto().FromEntity(board);
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
        var game = createGameDto.ToGame();
        Game newGame = appRepository.CreateGame(game);
        return new GameDto().FromEntity(newGame);
    }

    public List<Game> GetAllGames()
    {
        return appRepository.GetAllGames().ToList();
    }

    public void UpdateGame(GameDto gameDto){
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
        throw new ArgumentException($"Game with ID {createWinnerDto.Boardid} does not exist.");
    
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
