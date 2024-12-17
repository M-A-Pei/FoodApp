using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApp.Models;

public class UserModel{
    public int Id {get; set;}
    public required string name {get; set;}
    public required string email {get; set;}
    public required string password {get; set;} 
}
