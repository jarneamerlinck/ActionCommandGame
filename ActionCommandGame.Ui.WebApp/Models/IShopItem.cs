using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.WebApp.Views.Home
{
    public class IShopItem
    {
        [Required] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string StatsCategory { get; set; }
        [Required] 
        public int StatsValue { get; set; }
        [Required]
        public int Price { get; set; }
        public string ImageLocation { get; set; }
        public string Description { get; set; }
        

    }
}
