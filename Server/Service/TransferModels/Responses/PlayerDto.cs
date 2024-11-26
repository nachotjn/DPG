using System.Dynamic;
using DataAccess.Models;

public class PlayerDto{
    public PlayerDto FromEntity(Player player){
        return new PlayerDto{
            PlayerId= player.Playerid,
            Name = player.Name,
            Email = player.Email,
            Phone = player.Phone,
            IsAdmin = player.Isadmin,
            IsActive = player.Isactive,
            Balance = player.Balance
        };
    }

    public Guid PlayerId{get;set;}
    public string? Name{get;set;}
    public string? Email{get;set;}
    public string? Phone { get; set; }
    public bool IsAdmin {get; set;} 
    public bool IsActive {get;set;}
    public decimal Balance { get; set; }
}