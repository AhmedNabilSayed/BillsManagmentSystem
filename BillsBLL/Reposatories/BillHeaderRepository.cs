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
    public class BillHeaderRepository : GenericReposatory<BillHeader>, IBillHeaderRepository
	{
		private readonly BillsDbContext _context;

		public BillHeaderRepository(BillsDbContext context) : base(context)
		{
			_context = context;
		}

        public async Task<BillHeader?> GetByInvoiceNoAsync(int? invoiceNo)
        {
            return await _context.BillHeaders.Where(b => (b.BILCOD == invoiceNo && !b.IsDeleted)).FirstOrDefaultAsync();
        }

        public async Task<List<BillHeader>> GetByVendor(int VndCod)
        {
            return await _context.BillHeaders.Where(c => c.VNDCOD == VndCod && c.IsDeleted == false).ToListAsync();
        }

        public int? GetLastId()
		{
			return _context.BillHeaders.OrderByDescending(o => o.BILCOD).Select(s =>s.BILCOD).FirstOrDefault();
		}
	}
}
