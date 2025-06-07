using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
    }
}
