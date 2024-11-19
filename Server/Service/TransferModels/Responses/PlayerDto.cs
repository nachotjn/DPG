using System.Dynamic;
using DataAccess.Models;

public class PlayerDto{
    public PlayerDto FromEntity(Player player){
        return new PlayerDto{
            PlayerId= player.Playerid,
            Name = player.Name,
            Email = player.Email
        };
    }

    public Guid PlayerId{get;set;}

    public string? Name{get;set;}
    public string? Email{get;set;}
}