using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
	public class SalesBillHeaderViewModel
	{
		[Required]
		public int BILCOD { get; set; }
		[Required]
		public string BILDAT { get; set; }
		[Required]
		public int CustomerId { get; set; }
		[Required]
		public int StoreId { get; set; }

		public decimal? BILPRC { get; set; }
		public string? BILIMG { get; set; }
		public IFormFile? Image { get; set; }
	}
}
