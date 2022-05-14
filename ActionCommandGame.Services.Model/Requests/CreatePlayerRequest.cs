using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Model.Requests
{
    public class CreatePlayerRequest
    {

        
        
        
        [DisplayName("Player Name")]
        public string Name { get; set; }
        public string ImageLocation { get; set; }
    }
}
