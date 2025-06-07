using BillsBLL.IReposatories;
using BillsDAL.Context;
using BillsEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.Reposatories
{
    public class SalesBillHeaderRepository : GenericReposatory<SalesBillHeader>, ISalesBillHeaderRepository
    {
        private readonly BillsDbContext context;

        public SalesBillHeaderRepository(BillsDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<SalesBillHeader?> GetByInvoiceNoAsync(int? invoiceNo)
        {
            return await context.SalesBillHeaders.Where(b => (b.BILCOD == invoiceNo && !b.IsDeleted)).FirstOrDefaultAsync();
        }

        public int? GetLastId()
        {
            return context.SalesBillHeaders.OrderByDescending(o => o.BILCOD).Select(s => s.BILCOD).FirstOrDefault();
        }
    }
}
