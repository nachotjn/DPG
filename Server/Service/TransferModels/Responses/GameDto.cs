using DataAccess.Models;

public class GameDto{
    public GameDto FromEntity(Game game){
        return new GameDto{
            GameID = game.Gameid,
            Weeknumber = game.Weeknumber,
            Year = game.Year,
            Winningnumbers = game.Winningnumbers,
            Iscomplete = game.Iscomplete,
            Prizesum = game.Prizesum,
        };
    }
    public Guid GameID{get;set;}
    public int Weeknumber { get; set; }

    public int Year { get; set; }

    public List<int>? Winningnumbers { get; set; }

    public bool Iscomplete { get; set; }

    public decimal? Prizesum { get; set; }
}