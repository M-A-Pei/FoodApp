using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;
namespace FoodApp.Controllers;

public class FoodController:Controller{

    private readonly ApplicationDbContext _context;
    public FoodController(ApplicationDbContext context){
        _context = context;
    }

    public IActionResult List(){
        var foods = _context.Foods.ToList();
        return View(foods);
    }

    public IActionResult AddFood()
    {
        return View();
    }

    public IActionResult FoodDetail(int id){
        FoodModel? food = _context.Foods.Find(id);
        return View(food);
    }

    public IActionResult EditFood(int id){
        FoodModel? food = _context.Foods.Find(id);
        if(food != null){
            return View(food);
        }
        return RedirectToAction("List");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFood(FoodModel food){
        if(ModelState.IsValid){
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
            
        }
        return View("List");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFood(int id){
        FoodModel? food = await _context.Foods.FindAsync(id);
        if(food != null){
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        return RedirectToAction("List");

    }

    [HttpPost]
    public async Task<IActionResult> UpdateFood(FoodModel food){
        if(ModelState.IsValid){
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        return View("List");
    }
}