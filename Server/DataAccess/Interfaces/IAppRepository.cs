using DataAccess.Models;

public interface IAppRepository{
    //Users
    public Player CreatePlayer(Player player);
    public List <Player> GetAllPlayers();
    public void UpdatePlayer(Player player);
    public Player? GetPlayerById(Guid playerId);
    public List<Player> GetPlayersForGame(Guid gameID);
    
    //Boards
    public Board CreateBoard(Board board);
    public List<Board> GetAllBoards();
    public List<Board> GetBoardsForPlayer(Guid playerID);
    public List<Board> GetBoardsForGame(Guid gameID);
    public Board? GetBoardByID(Guid boardId);

    //Games
    public Game CreateGame(Game game);
    public List<Game> GetAllGames();
    public Game? GetGameById(Guid gameID);
    
    //Winners
    public Winner CreateWinner(Winner winner);
    public List<Winner> GetAllWinners();

    //Player Balance
    public Playerbalance CreatePlayerBalance(Playerbalance playerbalance);
    public Playerbalance GetPlayerBalanceForPlayer(Guid playerId);

}