using System.Dynamic;
using DataAccess.Models;

public class PlayerDto{
    public PlayerDto FromEntity(Player player){
        return new PlayerDto{
            PlayerId= player.Playerid,
            Name = player.Name,
            Email = player.Email,
            IsAdmin = player.Isadmin,
            IsActive = player.Isactive,
            AnnualFeePaid = player.Annualfeepaid
        };
    }

    public Guid PlayerId{get;set;}

    public string? Name{get;set;}
    public string? Email{get;set;}

    public bool IsAdmin {get; set;} 

    public bool IsActive {get;set;}

    public bool AnnualFeePaid { get; set; }
}