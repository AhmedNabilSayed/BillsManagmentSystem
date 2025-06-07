using BillsDAL.Reposatories;
using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.IReposatories
{
	public interface IBillHeaderRepository : IgenericReposatory<BillHeader>
	{
		int? GetLastId();

		Task<BillHeader?> GetByInvoiceNoAsync(int? invoiceNo);
		Task<List<BillHeader>> GetByVendor(int VndCod);

    }
}
