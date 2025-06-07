using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
	public class SignInViewModel
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public bool RememberMe { get; set; } = false;
	}
}
