using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
	public class VendorViewModel
	{
		public int VndCod { get; set; }
		[Required(ErrorMessage ="Name Is Required")]
		public string VndNam { get; set; }
	}
}
