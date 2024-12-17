namespace FoodApp.DTO.Food;

public class CreateFood{
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IFormFile Image { get; set; }
        public required int Price { get; set; }
}