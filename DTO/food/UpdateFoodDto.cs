namespace FoodApp.DTO.Food;

public class UpdateFood{
    public required int Id {get; set;}
    public required string Name { get; set; }
    public required string Description { get; set; }
    public IFormFile? Image { get; set; }
    public required int Price { get; set; }
}