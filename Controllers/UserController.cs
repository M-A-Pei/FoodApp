using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;

namespace FoodApp.Controllers;

public class UserController:Controller{
    private readonly ApplicationDbContext _context;
    public UserController(ApplicationDbContext context){
        _context = context;
    }

    [HttpGet]
    public UserModel? GetUser(int id){
       UserModel? user = _context.Users.Find(id);
       return user;
    }

    [HttpGet]
    public List<UserModel> FindUsers(){
        List<UserModel> res = _context.Users.ToList();
        return res;
    }

    [HttpGet]
    public UserModel? GetUserByEmail(string email){
        UserModel? res = _context.Users.SingleOrDefault(u => u.email == email);
        return res;
    }
}