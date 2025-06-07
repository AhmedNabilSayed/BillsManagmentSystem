using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
	public class ItemViewModel
	{
		public int ItmCod { get; set; }
		[Required(ErrorMessage ="Name is required")]
		public string ItmNam { get; set; }
		[Required(ErrorMessage = "price is required")]
		[Range(1, double.MaxValue, ErrorMessage = "The price should not be less than 1")]
		public decimal ItmPrc { get; set; }
	}
}
