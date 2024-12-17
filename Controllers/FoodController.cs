using Microsoft.AspNetCore.Mvc;
using FoodApp.Data;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.DTO.Food;
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
    public async Task<IActionResult> CreateFood(CreateFood food){ //bind the request body to fit the FoodModel

        if (food.Image == null || food.Image.Length == 0) //check image serperatly from rest of model
        {
           ModelState.AddModelError("Image", "The Image field is required.");
           return BadRequest("upload image is required");
        }

        if(ModelState.IsValid){ //check if the rest of the model is valid

            var response = await _uploadService.UploadFileAsync(food.Image); //upoad the image then get the url and publicId, using the UploadService previously created
            string? ImageUrl = response.url;
            string? ImagePublicId = response.publicId;
            if(ImageUrl == null || ImagePublicId == null){
                return BadRequest("failed to upload image");
            }
            FoodModel insertFood = new FoodModel{
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                ImageUrl = ImageUrl,
                ImagePublicId = ImagePublicId
            };
            _context.Foods.Add(insertFood);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
            
        }

        foreach (var error in ModelState.Values.SelectMany(v => v.Errors)){
                Console.WriteLine(error.ErrorMessage);
        }
        return View("List");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFood(int id){
        FoodModel? food = await _context.Foods.FindAsync(id);
        if(food != null && food.ImagePublicId != null){
            _context.Foods.Remove(food);
            bool res = await _uploadService.DeleteFileAsync(food.ImagePublicId);
            if(!res){ 
                return BadRequest("failed to delete image");                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        return RedirectToAction("List");

    }

    [HttpPost]
    public async Task<IActionResult> UpdateFood(UpdateFood food){
        FoodModel? existingFood = await _context.Foods.FindAsync(food.Id);
        if(existingFood != null){
            existingFood.Name = food.Name;
            existingFood.Description = food.Description;
            existingFood.Price = food.Price;
            Console.WriteLine("updating data");       

            if (food.Image != null && existingFood.ImagePublicId != null)
            {
                Console.WriteLine("uploading image");
                await _uploadService.DeleteFileAsync(existingFood.ImagePublicId);
                var response = await _uploadService.UploadFileAsync(food.Image);
                string? url = response.url;
                string? publicId = response.publicId;
                if (url == null || publicId == null)
                {
                    return BadRequest("failed to upload image");
                }
                existingFood.ImagePublicId = publicId;
                existingFood.ImageUrl = url;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        return View("List");
    }
}