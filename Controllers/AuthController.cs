using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;
using FoodApp.DTO.User;

namespace FoodApp.Controllers;

public class AuthController:Controller{
    private readonly ApplicationDbContext _context;
    public AuthController(ApplicationDbContext context){
        _context = context;
    }
    
    [HttpGet("Login")]
    public IActionResult Login(){
        return View();
    }

    [HttpGet("Register")]
    public IActionResult Register(){
        return View();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> AddUser(CreateUser user){
        if(ModelState.IsValid){
            UserModel data = new UserModel{
                name = user.name,
                email = user.email,
                password = user.password 
            };
            _context.Users.Add(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        return BadRequest("failed to make user, model isnt valid");
    }

    [HttpPost("Login")]
    public IActionResult LoginLogic(Login login){
        if(ModelState.IsValid){

        }
        return RedirectToAction("Login");
    }
}