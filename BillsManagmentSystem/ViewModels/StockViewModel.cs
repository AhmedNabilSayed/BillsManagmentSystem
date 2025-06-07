using BillsEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillsManagmentSystem.ViewModels
{
	public class StockViewModel
	{
		public int Id { get; set; }
		public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int ItemId { get; set; }
		public string ItemName { get; set; }
		public int ItemQuantity { get; set; }
	}
}
