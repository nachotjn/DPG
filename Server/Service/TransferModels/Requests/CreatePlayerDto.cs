using DataAccess.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


public class CreatePlayerDto{
    [Required(ErrorMessage ="Name Is required")]
    public string Name{get; set;} = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }

    [Required]
    [PasswordPropertyText]
    public required string Password{get;set;}

    public Player ToPlayer(){
        return new Player{
            Name = Name,
            Email = Email,
            Password = Password
        };
    }

}