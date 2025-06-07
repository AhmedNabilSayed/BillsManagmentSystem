using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.IReposatories
{
	public interface IUnitOfWork
	{
		public IBillHeaderRepository BillHeaderRepository { get; set; }
		public IBillDetailsRepository BillDetailsRepository { get; set; }
		public IVendorRepository VendorRepository { get; set; }
		public IItemsRepository ItemsRepository { get; set; }
		public IStoresRepository StoresRepository { get; set; }
		public IStockRepository StockRepository { get; set; }
		public ISalesBillHeaderRepository SalesBillHeaderRepository { get; set; }
		public ISalesBillDetailsRepository SalesBillDetailsRepository { get; set; }
		public ICustomerRepository CustomerRepository { get; set; }
	}
}
