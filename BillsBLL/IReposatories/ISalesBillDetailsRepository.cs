using BillsDAL.Reposatories;
using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.IReposatories
{
    public interface ISalesBillDetailsRepository : IgenericReposatory<SalesBillDetails>
    {
        Task<List<SalesBillDetails>?> GetByInvoiceNoAsync(int? invoiceNo);
    }
}
