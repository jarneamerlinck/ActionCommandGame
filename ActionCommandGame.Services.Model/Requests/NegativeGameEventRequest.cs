using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Abstractions;

namespace ActionCommandGame.Services.Model.Requests
{
    public class NegativeGameEventRequest: IHasProbability
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string DefenseWithGearDescription { get; set; }
        [Required]
        public string DefenseWithoutGearDescription { get; set; }
        [Required]
        public int DefenseLoss { get; set; }
        [Required]
        public int Probability { get; set; }
    }
}
