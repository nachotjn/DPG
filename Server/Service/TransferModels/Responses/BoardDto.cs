using DataAccess.Models;

public class BoardDto{
    public BoardDto()
    {
        Numbers = new List<int>(); 
    }
    public BoardDto FromEntity(Board board){
        return new BoardDto{
            BoardId = board.Boardid,
            Numbers = board.Numbers ?? new List<int>(),
            Isautoplay = board.Isautoplay,
            Autoplayweeks = board.Autoplayweeks,
            Playerid = board.Playerid,
            Gameid = board.Gameid,
            Game = board.Game,
            Player = board.Player
        };
    }
    public Guid BoardId {get;set;}
    [MinMaxNumbers(5, 8)]
    public List<int> Numbers { get; set; } = new();
    public bool Isautoplay { get; set; }
    public int? Autoplayweeks { get; set; }
    public Guid Playerid { get; set; }
    public Guid Gameid { get; set; }
    public virtual Game Game { get; set; } = null!;
    public virtual Player Player { get; set; } = null!;
}