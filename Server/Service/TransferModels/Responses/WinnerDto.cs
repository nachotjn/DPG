using DataAccess.Models;

public class WinnerDto{
    public WinnerDto FromEntity(Winner winner){
        return new WinnerDto{
            Winnerid = winner.Winnerid,
            Playerid = winner.Playerid,
            Gameid = winner.Gameid,
            Boardid = winner.Boardid,
            Winningamount = winner.Winningamount,
            Board = winner.Board,
            Game = winner.Game,
            Player = winner.Player
        };
    }
     public Guid Winnerid { get; set; }

    public Guid Playerid { get; set; }

    public Guid Gameid { get; set; }

    public Guid Boardid { get; set; }

    public decimal Winningamount { get; set; }
    public virtual Board Board { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}