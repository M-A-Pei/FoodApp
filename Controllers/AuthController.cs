using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;

namespace FoodApp.Controllers;

public class AuthController:Controller{
    private readonly ApplicationDbContext _context;
    public AuthController(ApplicationDbContext context){
        _context = context;
    }
    
    public IActionResult Login(){
        return View();
    }

    public IActionResult Register(){
        return View();
    }

    public async Task<IActionResult> AddUser(UserModel user){
        if(ModelState.IsValid){
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        return BadRequest("failed to make user, model isnt valid");
    }
}