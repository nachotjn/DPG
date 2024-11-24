
using DataAccess.Models;

public class CreateBoardDto{
    public required List<int> Numbers{get;set;}
    public bool Isautoplay { get; set; }
    public Guid Playerid { get; set; }
    public Guid Gameid { get; set; }
 

    public Board ToBoard(){
        return new Board{
            Numbers = Numbers,
            Isautoplay = Isautoplay,
            Playerid = Playerid,
            Gameid = Gameid
        };
    }
}