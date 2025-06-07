using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.BillHeader_Specifications
{
    public class BillHeaderWithSpec : BaseSpecification<BillHeader>
    {
        public int StoreId { get; set; }
        public BillHeaderWithSpec():base(d => d.IsDeleted == false)
        {
            Includes.Add(b => b.Vendor);
            AddOrderByDescending(o => o.BILCOD);
        }   

        public BillHeaderWithSpec(int id) : base(b => b.BILCOD == id &&!b.IsDeleted)
        {
            Includes.Add(b => b.Vendor);
        }

    }
}
