using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(15 ,MinimumLength = 6 , ErrorMessage ="the password should be between 6 to 15 character")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password) , ErrorMessage ="Password Mismatch")]
        public string ConfirmPassword { get; set; }

    }
}
