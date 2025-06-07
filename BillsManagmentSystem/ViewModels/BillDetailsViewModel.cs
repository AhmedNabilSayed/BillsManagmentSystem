using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
	public class BillDetailsViewModel
	{
		[Required]
		public int DTLCOD { get; set; }
		[Required]
		public int ITMCOD { get; set; }
		[Required]
		public decimal ITMPRC { get; set; }
		[Required]
		public int ITMQTY { get; set; }
		private decimal? total;

		public decimal? Total
		{
			get { return ITMPRC * ITMQTY; }
		}

		[Required]
		public int BILCOD { get; set; }
		[Required]
		public bool IsDeleted { get; set; } = false;
	}
}
