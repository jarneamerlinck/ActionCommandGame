using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Model.Requests
{
    public class CreatePlayerRequest
    {

        
        
        public int Id { get; set; }
        [DisplayName("Player Name")]
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ImageLocation { get; set; }
    }
}
