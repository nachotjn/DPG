using DataAccess.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


public class CreatePlayerDto{

    [Required(ErrorMessage ="Name Is required")]
    public string Name {get; set;} = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required]
    [Phone(ErrorMessage ="Invalid phone number")]
    public string? Phone { get; set; }

    [Required]
    public required string Password{get;set;}

    public bool IsAdmin {get; set;} 

    public bool IsActive {get;set;}

    public decimal Balance { get; set; }

    public Player ToPlayer(){
        return new Player{
            UserName = Name,
            Email = Email,
            PhoneNumber = Phone,
           Isadmin = IsAdmin,
           Isactive = IsActive,
           Balance = Balance
        };
    }

}