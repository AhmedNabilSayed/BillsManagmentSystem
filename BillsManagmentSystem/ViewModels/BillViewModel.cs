using System.ComponentModel.DataAnnotations;

namespace BillsManagmentSystem.ViewModels
{
    public class BillViewModel
    {
        public string InvoiceNo { get; set; }
        public string VndCod { get; set; }
        public string StoreId { get; set; }
        public string BILDAT { get; set; }
        public string? BILPRC { get; set; }
		public IFormFile? Image { get; set; }
        public string? BILIMG { get; set; }
		public string? listproducts { get; set; }
        public List<product>? products { get; set; }
    }
    public class product
    {
		public string ITMPRC { get; set; }
		public string ITMQTY { get; set; }
		public string ITMCOD { get; set; }
		public decimal? Total { get; set; }

	}
}
