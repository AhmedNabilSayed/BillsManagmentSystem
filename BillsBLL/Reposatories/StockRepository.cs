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
    public class StockRepository : GenericReposatory<Stock>, IStockRepository
	{
		private readonly BillsDbContext _context;

		public StockRepository(BillsDbContext context) : base(context)
		{
			_context = context;
		}

		
	}
}
