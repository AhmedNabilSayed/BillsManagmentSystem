using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
	public class ChangePasswordViewModel
	{
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
