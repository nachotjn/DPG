using DataAccess.Models;

namespace Service;

public interface IAppService{
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto);
    public List<Player> GetAllPlayers();
    public void UpdatePlayer(PlayerDto playerDto);

    public BoardDto CreateBoard(CreateBoardDto createBoardDto);
    public List<Board> GetAllBoards();

    
    public GameDto CreateGame(CreateGameDto createGameDto);
    public List<Game> GetAllGames();
    public void UpdateGame(GameDto gameDto);


    public WinnerDto CreateWinner(CreateWinnerDto createWinnerDto);
    public List<Winner> GetAllWinners();


    public TransactionDto CreateTransaction(CreateTransactionDto createTransactionDto);
}

public class AppService(IAppRepository appRepository) : IAppService{
    //Players
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        var player = createPlayerDto.ToPlayer();
        Player newPlayer = appRepository.CreatePlayer(player);
        return new PlayerDto().FromEntity(newPlayer);
    }

    public List<Player> GetAllPlayers()
    {
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


    
}
