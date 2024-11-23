using DataAccess.Models;

public class CreateWinnerDto{
    public Guid Playerid { get; set; }

    public Guid Gameid { get; set; }

    public Guid Boardid { get; set; }

    public decimal Winningamount { get; set; }

    public Winner ToWinner(){
        return new Winner{
            Playerid = Playerid,
            Gameid = Gameid,
            Boardid = Boardid,
            Winningamount = Winningamount
        };
    }
}