using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;
using FoodApp.Services;
namespace FoodApp.Controllers;

public class FoodController:Controller{

    private readonly ApplicationDbContext _context;
    private readonly IUploadService _uploadService;
    public FoodController(ApplicationDbContext context, IUploadService uploadService){
        _context = context;
        _uploadService = uploadService;
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
        Console.WriteLine(food.Image);

        if (food.Image == null || food.Image.Length == 0)
        {
           ModelState.AddModelError("Image", "The Image field is required.");
           return BadRequest("upload image is required");
        }

        if(ModelState.IsValid){
            var url = await _uploadService.UploadFileAsync(food.Image);
            if(url == null){
                Console.WriteLine("upload failed yooooooooooooo");
                return BadRequest("failed to upload image");
            }
            Console.WriteLine("upload success yooooooooooooo");
            food.ImageUrl = url;
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
            
        }
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors)){
                Console.WriteLine(error.ErrorMessage);
        }
        Console.WriteLine("model isnt valid yooooooooooooo");
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