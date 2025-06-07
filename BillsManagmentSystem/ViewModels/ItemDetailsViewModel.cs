using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
    public class ItemDetailsViewModel
    {
        [Required]
        public int DTLCOD { get; set; }
        [Required]
        public int ITMQTY { get; set; }
        [Required]
        public int ITMPRC { get; set; }
    }
}
