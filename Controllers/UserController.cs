using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;
using FoodApp.Services;

namespace FoodApp.Controllers;

public class UserController:Controller{
    private readonly UserService _userService;
    private readonly ApplicationDbContext _context;
    public UserController(ApplicationDbContext context, UserService userService){
        _userService = userService;
        _context = context;
    }

    [HttpGet]
    public UserModel? GetUser(int id){
       return _userService.GetUser(id);
    }

    [HttpGet]
    public List<UserModel> FindUsers(){
        return _userService.FindUsers();
    }

    [HttpGet]
    public UserModel? GetUserByEmail(string email){
        return _userService.GetUserByEmail(email);
    }
}