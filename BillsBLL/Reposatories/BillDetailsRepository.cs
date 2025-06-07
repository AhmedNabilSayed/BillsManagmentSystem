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
    public class BillDetailsRepository : GenericReposatory<BillDetails>, IBillDetailsRepository
	{
		private readonly BillsDbContext _context;

		public BillDetailsRepository(BillsDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<BillDetails>?> GetByInvoiceNoAsync(int? invoiceNo)
		{
			return await _context.BillDetails.Include(b => b.Item).Include(b => b.BillHeader).Where(b => (b.BILCOD == invoiceNo && !b.IsDeleted) ).ToListAsync();	
		}

     
    }
}
