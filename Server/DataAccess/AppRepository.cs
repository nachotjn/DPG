using DataAccess;
using DataAccess.Models;

public class AppRepository(AppDbContext context) : IAppRepository
{

    //Players
    public Player CreatePlayer(Player player)
    {
        context.Players.Add(player);
        context.SaveChanges();
        return player;
    }

    public List<Player> GetAllPlayers()
    {
        return context.Players.ToList();
    }

     public Player? GetPlayerById(Guid playerId)
    {
        throw new NotImplementedException();
    }

    public void UpdatePlayer(Player player)
    {
        throw new NotImplementedException();
    }


    public Board CreateBoard(Board board)
    {
        throw new NotImplementedException();
    }

    

    public List<Board> GetAllBoards()
    {
        throw new NotImplementedException();
    }

    

    public List<Board> GetBoardForPlayer(Guid playerID)
    {
        throw new NotImplementedException();
    }

   
}