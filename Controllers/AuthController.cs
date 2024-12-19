using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;
using FoodApp.DTO.User;
using FoodApp.Services;
using Microsoft.AspNetCore.Http;

namespace FoodApp.Controllers;

public class AuthController:Controller{
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly JwtService _jwtService;
    public AuthController(ApplicationDbContext context, UserService userService, JwtService jwtService){
        _userService = userService;
        _context = context;
        _jwtService = jwtService;
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
            UserModel? user = _userService.GetUserByEmail(login.email);
            if(user != null && user.password == login.password){
                string token = _jwtService.GenerateToken(user.Id.ToString());

                Response.Cookies.Append("jwt", token, new CookieOptions{
                    HttpOnly = true,    // Prevents JavaScript from accessing the cookie
                    Secure = true,      // Ensures the cookie is sent only over HTTPS
                    SameSite = SameSiteMode.Strict, // Prevents CSRF attacks
                    Expires = DateTime.UtcNow.AddHours(1) // Token expiration
                });

                return RedirectToAction("Index", "Home");
            }
        }
        return BadRequest("incorrect form data");
    }
}