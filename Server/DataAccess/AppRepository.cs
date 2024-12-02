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
        var players = context.Boards
        .Where(b => b.Gameid == gameID)
        .Select(b => b.Player)
        .Distinct()
        .Include(p => p.Boards)         
        .ToList();

        return players;
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
        return context.Boards
        .Include(b => b.Game)
        .Include(b => b.Player)
        .Where( b => b.Playerid == playerID).ToList();
    }

    public List<Board> GetBoardsForGame(Guid gameID){
        return context.Boards
        .Include(b => b.Game)
        .Include(b => b.Player)
        .Where( b => b.Gameid == gameID).ToList();
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
        var existingGame = context.Games.Find(game.Gameid);
        if(existingGame == null){
            throw new Exception($"Game with ID {game.Gameid} not found.");
        }
        
        existingGame.Iscomplete = game.Iscomplete;
        existingGame.Winningnumbers = game.Winningnumbers;
        existingGame.Prizesum = game.Prizesum;
        existingGame.Updatedat = game.Updatedat;

        context.Games.Update(existingGame);
        context.SaveChanges();
    }

    public Game? GetGameById(Guid gameID)
    {
        return context.Games.Include(g => g.Boards)
        .Include(g => g.Winners)
        .FirstOrDefault(g => g.Gameid == gameID);
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
        .Include(w => w.Game)
        .Include(w => w.Player)
        .Include(w=> w.Board)
        .ToList();
    }

    public List<Winner> GetWinnersForGame(Guid gameId)
    {
        return context.Winners
        .Include(w => w.Game)
        .Include(w => w.Player)
        .Include(w=> w.Board)
        .Where( w => w.Gameid == gameId).ToList();
    }




    //Transactions
    public Transaction CreateTransaction(Transaction transaction)
    {
        context.Transactions.Add(transaction);
        context.SaveChanges();
        return transaction;
    }

    public List<Transaction> GetTransactionsForPlayer(Guid playerId)
    {
        return context.Transactions
        .Include(t => t.Player)
        .Where( t => t.Playerid == playerId).ToList();
    }

    public void UpdateTransaction(Transaction transaction){
        var existingTransaction = context.Transactions.Find(transaction.Transactionid);
        if(existingTransaction == null){
            throw new Exception($"Transaction with ID {transaction.Transactionid} not found.");
        }

        existingTransaction.Isconfirmed = transaction.Isconfirmed;

        context.Transactions.Update(existingTransaction);
        context.SaveChanges();
    }

    
}