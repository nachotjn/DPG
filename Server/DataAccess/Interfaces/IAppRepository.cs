using DataAccess.Models;

public interface IAppRepository{
    //Users
    public Player CreatePlayer(Player player);
    public List <Player> GetAllPlayers();
    public void UpdatePlayer(Player player);
    public Player? GetPlayerById(Guid playerId);
    
    //Boards
    public Board CreateBoard(Board board);
    public List<Board> GetAllBoards();
    public List<Board> GetBoardForPlayer(Guid playerID);
    

}