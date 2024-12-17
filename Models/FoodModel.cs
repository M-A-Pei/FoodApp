using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApp.Models

{
    public class FoodModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Column("Image")]
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public required int Price { get; set; }
    }
}