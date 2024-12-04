
using DataAccess.Models;

public class CreateBoardDto{
    [MinMaxNumbers(5, 8)]
    public required List<int> Numbers{get;set;}
    public bool Isautoplay { get; set; }
    public int? Autoplayweeks { get; set; }
    public Guid Playerid { get; set; }
    public Guid Gameid { get; set; }
 

    public Board ToBoard(){
        return new Board{
            Numbers = Numbers,
            Isautoplay = Isautoplay,
            Autoplayweeks = Autoplayweeks,
            Playerid = Playerid,
            Gameid = Gameid
        };
    }
}