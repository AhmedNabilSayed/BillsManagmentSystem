using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.SalesBillSpecifications
{
	public class SalesBillHeaderWithSpec : BaseSpecification<SalesBillHeader>
	{
		public SalesBillHeaderWithSpec() : base(d => d.IsDeleted == false)
		{
			Includes.Add(b => b.Customer);
			AddOrderByDescending(o => o.BILCOD);
		}

		public SalesBillHeaderWithSpec(int id) : base(b => b.BILCOD == id && !b.IsDeleted)
		{
			Includes.Add(b => b.Customer);
		}
	}
}
