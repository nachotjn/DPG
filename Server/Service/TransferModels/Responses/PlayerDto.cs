using System.ComponentModel.DataAnnotations;
using DataAccess.Models;


public class PlayerDto{
    public PlayerDto FromEntity(Player player){
        if (player == null) throw new ArgumentNullException(nameof(player));
        return new PlayerDto{
            PlayerId= player.Id,
            Name = player.UserName,
            Email = player.Email,
            Phone = player.PhoneNumber,
            IsAdmin = player.Isadmin,
            IsActive = player.Isactive,
            Balance = player.Balance
        };
    }

    public Guid PlayerId{get;set;}

    [Required(ErrorMessage ="Name Is required")]
    public string? Name{get;set;}

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email{get;set;}

    [Required]
    [Phone(ErrorMessage ="Invalid phone number")]
    public string? Phone { get; set; }
    public bool IsAdmin {get; set;} 
    public bool IsActive {get;set;}
    public decimal Balance { get; set; }
    public DateTime? Updatedat { get; set; }
    public DateTime? ToDatabaseKind(DateTime? input)
    {
        return input.HasValue ? DateTime.SpecifyKind(input.Value, DateTimeKind.Unspecified) : (DateTime?)null;
    }
}