using BillsBLL.IReposatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.Reposatories
{
	public class UnitOfWork : IUnitOfWork
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

        public UnitOfWork(IBillHeaderRepository billHeaderRepository
						, IBillDetailsRepository billDetailsRepository
                        , IVendorRepository vendorRepository
                        , IItemsRepository itemsRepository
                        , IStoresRepository storesRepository
                        , IStockRepository stockRepository
                        , ISalesBillHeaderRepository salesBillHeaderRepository
                        , ISalesBillDetailsRepository salesBillDetailsRepository
                        , ICustomerRepository customerRepository)
        {
            BillHeaderRepository = billHeaderRepository;
            BillDetailsRepository = billDetailsRepository;
            VendorRepository = vendorRepository;
            ItemsRepository = itemsRepository;
            StoresRepository = storesRepository;
            StockRepository = stockRepository;
            SalesBillHeaderRepository = salesBillHeaderRepository;
            SalesBillDetailsRepository = salesBillDetailsRepository;
            CustomerRepository = customerRepository;
        }
    }
}
