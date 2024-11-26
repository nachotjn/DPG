using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

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
        return context.Players
        .Include(p => p.Boards)
        .Include(p => p.Transactions)
        .Include(p => p.Winners)
        .ToList();
    }

     public Player? GetPlayerById(Guid playerId)
    {
        return context.Players.Include(p => p.Boards)
        .Include(p => p.Transactions)
        .Include(p => p.Winners)
        .FirstOrDefault(p => p.Playerid == playerId);
    }

    public void UpdatePlayer(Player player)
    {
        var existingPlayer = context.Players.Find(player.Playerid);
        if(existingPlayer == null){
            throw new Exception($"Player with ID {player.Playerid} not found.");
        }

        existingPlayer.Name = player.Name;
        existingPlayer.Email = player.Email;
        existingPlayer.Password = player.Password;
        existingPlayer.Phone = player.Phone;
        existingPlayer.Isadmin = player.Isadmin;
        existingPlayer.Isactive = player.Isactive;
        existingPlayer.Balance = player.Balance;
        existingPlayer.Updatedat = player.Updatedat;

        context.Players.Update(existingPlayer);
        context.SaveChanges();
    }

    public List<Player> GetPlayersForGame(Guid gameID)
    {
        throw new NotImplementedException();
    }

    public void DeletePlayer(Guid PlayerId){
        throw new NotImplementedException();
    }



    //Boards
    public Board CreateBoard(Board board)
    {
        context.Boards.Add(board);
        context.SaveChanges();
        return board;
    }

    public List<Board> GetAllBoards()
    {
        return context.Boards
        .Include(b => b.Game)
        .Include(b => b.Player)
        .ToList();
    }

    public List<Board> GetBoardsForPlayer(Guid playerID)
    {
        return context.Boards.Where( b => b.Playerid == playerID).ToList();
    }

    public List<Board> GetBoardsForGame(Guid gameID){
        return context.Boards.Where( b => b.Gameid == gameID).ToList();
    }

    

    public Board? GetBoardByID(Guid boardId){
        return context.Boards
        .Include(b => b.Game)
        .Include(b => b.Player)
        .FirstOrDefault(b => b.Boardid == boardId);
    }



    //Games
    public Game CreateGame(Game game)
    {
         context.Games.Add(game);
         context.SaveChanges();
         return game;
    }

    public List<Game> GetAllGames()
    {
        return [.. context.Games];
    }

    public void UpdateGame(Game game){
        throw new NotImplementedException();
    }

    public Game? GetGameById(Guid gameID)
    {
        return context.Games.Include(p => p.Boards)
        .Include(p => p.Winners)
        .FirstOrDefault(g => g.Gameid == gameID);
    }

    public List<Winner> GetWinnersForGame(Guid GameId)
    {
        throw new NotImplementedException();
    }


    //Winners

    public Winner CreateWinner(Winner winner)
    {
        context.Winners.Add(winner);
        context.SaveChanges();
        return winner;
    }
    
    public List<Winner> GetAllWinners()
    {
        return context.Winners
        .Include(b => b.Game)
        .Include(b => b.Player)
        .Include(b=> b.Board)
        .ToList();
    }

    //Transactions

    public Transaction CreateTransaction(Transaction transaction)
    {
        context.Transactions.Add(transaction);
        context.SaveChanges();
        return transaction;
    }

    public Transaction GetPlayerTransactionsForPlayer(Guid playerId)
    {
        throw new NotImplementedException();
    }

    
}