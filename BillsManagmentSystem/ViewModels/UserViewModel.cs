using DevExpress.XtraPrinting;
using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageProfile { get; set; }
    }
}
