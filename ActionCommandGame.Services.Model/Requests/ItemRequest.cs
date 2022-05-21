using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Services.Model.Requests;

namespace ActionCommandGame.Services.Model.Requests
{
    public class ItemRequest
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Description { get; set; }
        public string? ImageLocation { get; set; }

        [Required] 
        public int Price { get; set; }
        public int Fuel { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        [Required] 
        public int ActionCooldownSeconds { get; set; }
    }
}
