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
    public class ItemsRepository : GenericReposatory<Items>, IItemsRepository
	{
		private readonly BillsDbContext _context;

		public ItemsRepository(BillsDbContext context) : base(context)
		{
			_context = context;
		}

		public IQueryable<Items> GetBySearch(string search)
		{
			return  _context.Items.Where( i => i.ItmNam.Trim().ToLower().Contains(search.Trim().ToLower()));	
		}
	}
}
