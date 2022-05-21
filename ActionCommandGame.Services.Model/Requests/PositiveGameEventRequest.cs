using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Abstractions;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PositiveGameEventRequest: IHasProbability
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Money { get; set; }
        [Required]
        public int Experience { get; set; }
        [Required]
        public int Probability { get; set; }
    }
}
