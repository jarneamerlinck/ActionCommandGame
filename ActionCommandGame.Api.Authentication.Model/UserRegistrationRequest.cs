using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Api.Authentication.Model
{
	public class UserRegistrationRequest
	{
		public string? Email { get; set; }

		public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
		public string? ReturnUrl { get; set; }
	}
}
