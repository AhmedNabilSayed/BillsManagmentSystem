using Microsoft.Build.Framework;

namespace BillsManagmentSystem.ViewModels
{
	public class BillHeaderViewModel
	{
		[Required]
		public int BILCOD { get; set; }
		[Required]
		public string BILDAT { get; set; }
		[Required]
		public int VNDCOD { get; set; }
		[Required]
        public int StoreId { get; set; }

        [Required]
		public decimal? BILPRC { get; set; }
		public string? BILIMG { get; set; }
		public IFormFile? Image { get; set; }
	}
}
