using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Ui.WebApp.Views.Home;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class ShopItem : IShopItem
    {
        
        public int? Attack { get; set; }
        public int? Defense { get; set; }
        public int? Fuel { get; set; }
        

    }
}
