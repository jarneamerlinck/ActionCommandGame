using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<Player> Players { set; get; }

    }
}
