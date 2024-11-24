using DataAccess.Models;

namespace Service;

public interface IAppService{
    public PlayerDto CreatePlayer(CreatePlayerDto createPlayerDto);
    public List<Player> GetAllPlayers();

    public BoardDto CreateBoard(CreateBoardDto createBoardDto);
    public List<Board> GetAllBoards();

    
    public GameDto CreateGame(CreateGameDto createGameDto);
    public List<Game> GetAllGames();


    public WinnerDto CreateWinner(CreateWinnerDto createWinnerDto);
    public List<Winner> GetAllWinners();


    public BalanceDto CreatePlayerBalance(CreateBalanceDto createBalanceDto);
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

    //Player Balance
    public BalanceDto CreatePlayerBalance(CreateBalanceDto createBalanceDto){
        var player = appRepository.GetPlayerById(createBalanceDto.Playerid);
        if (player == null)
        throw new ArgumentException($"Player with ID {createBalanceDto.Playerid} does not exist.");

        var balance = createBalanceDto.ToBalance();
        balance.Player = player;
        Playerbalance newBalance = appRepository.CreatePlayerBalance(balance);
        return new BalanceDto().FromEntity(balance);
    }


    
}
