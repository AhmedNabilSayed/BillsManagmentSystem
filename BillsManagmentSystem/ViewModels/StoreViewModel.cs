using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
    public class StoreViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Location { get; set; }
    }
}
