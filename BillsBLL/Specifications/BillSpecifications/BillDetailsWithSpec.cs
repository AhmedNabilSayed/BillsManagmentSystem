using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.BillSpecifications
{
    public class BillDetailsWithSpec : BaseSpecification<BillDetails>
    {
        public BillDetailsWithSpec() : base(d => d.IsDeleted == false)
        {
            Includes.Add(b => b.Item);
            Includes.Add(b => b.BillHeader);
        }

        public BillDetailsWithSpec(int id) : base(b => (b.DTLCOD == id && !b.IsDeleted))
        {
            Includes.Add(b => b.Item);
            Includes.Add(b => b.BillHeader);
        }

    }
}
