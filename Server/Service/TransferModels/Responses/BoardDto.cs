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
            Playerid = board.Playerid,
            Gameid = board.Gameid,
            Game = board.Game,
            Player = board.Player
        };
    }
    public Guid BoardId {get;set;}
    public List<int> Numbers { get; set; } = new();
    public bool Isautoplay { get; set; }
    public Guid Playerid { get; set; }
    public Guid Gameid { get; set; }
    public virtual Game Game { get; set; } = null!;
    public virtual Player Player { get; set; } = null!;
}