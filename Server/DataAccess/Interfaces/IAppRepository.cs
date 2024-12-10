using DataAccess.Models;

public interface IAppRepository{
    //Users
    public  Task<Player> CreatePlayer(Player player, string password);
    public List <Player> GetAllPlayers();
    public void UpdatePlayer(Player player);
    public Player? GetPlayerById(Guid playerId);
    public List<Player> GetPlayersForGame(Guid gameID);
    public void DeletePlayer (Guid PlayerId);
    
    //Boards
    public Board CreateBoard(Board board);
    public List<Board> GetAllBoards();
    public List<Board> GetBoardsForPlayer(Guid playerID);
    public List<Board> GetBoardsForGame(Guid gameID);
    public Board? GetBoardByID(Guid boardId);

    //Games
    public Game CreateGame(Game game);
    public List<Game> GetAllGames();
    public void UpdateGame(Game game);
    public Game? GetGameById(Guid gameID);
    public Game? GetNextGame(Game currentGame);
    public Game? GetGameByWeekAndYear(int weekNumber, int Year);
    
    //Winners
    public Winner CreateWinner(Winner winner);
    public List<Winner> GetAllWinners();
    public List<Winner> GetWinnersForGame(Guid gameId);
    public void UpdateWinner(Winner winner);
    public Winner? GetWinnerById(Guid winnerId);


    //Transactions
    public Transaction CreateTransaction(Transaction transaction);
    public List<Transaction> GetTransactionsForPlayer(Guid playerId);
    public void UpdateTransaction(Transaction transaction);
    public Transaction? GetTransactionById(Guid transactionId);

}