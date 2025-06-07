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
    public class SalesBillDetailsRepository : GenericReposatory<SalesBillDetails>, ISalesBillDetailsRepository
    {
        private readonly BillsDbContext _context;

        public SalesBillDetailsRepository(BillsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SalesBillDetails>?> GetByInvoiceNoAsync(int? invoiceNo)
        {
            return await _context.SalesBillDetails.Include(b => b.Item).Include(b => b.BillHeader).Where(b => (b.BILCOD == invoiceNo && !b.IsDeleted)).ToListAsync();
        }

        
    }
}
