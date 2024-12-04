using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

public class CreateGameDto{
    public int Weeknumber { get; set; }

    public int Year { get; set; }
  
    [MinMaxNumbers(0, 3)]
    public List<int>? Winningnumbers { get; set; }

    public bool Iscomplete { get; set; }

    public decimal? Prizesum { get; set; }

    public Game ToGame(){
        return new Game{
            Weeknumber = Weeknumber,
            Year = Year,
            Winningnumbers = Winningnumbers,
            Iscomplete = Iscomplete,
            Prizesum = Prizesum,
        };
    }

}