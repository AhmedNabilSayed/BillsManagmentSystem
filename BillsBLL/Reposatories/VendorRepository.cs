using BillsBLL.IReposatories;
using BillsDAL.Context;
using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsBLL.Reposatories
{
    public class VendorRepository : GenericReposatory<Vendor>, IVendorRepository
	{
		public VendorRepository(BillsDbContext context) : base(context)
		{
		}
	}
}
