using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.SalesBillSpecifications
{
	public class SalesBillDetailsWithSpec : BaseSpecification<SalesBillDetails>
	{
		public SalesBillDetailsWithSpec() : base(d => d.IsDeleted == false)
		{
			Includes.Add(b => b.Item);
			Includes.Add(b => b.BillHeader);
		}

		public SalesBillDetailsWithSpec(int id) : base(b => (b.DTLCOD == id && !b.IsDeleted))
		{
			Includes.Add(b => b.Item);
			Includes.Add(b => b.BillHeader);
		}
	}
}
