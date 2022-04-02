using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class ShopItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
        public int? Attack { get; set; }
        public int? Defense { get; set; }
        public int? Fuel { get; set; }
        
    }
}
