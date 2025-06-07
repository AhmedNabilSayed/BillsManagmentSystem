using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.VendorSpecifications
{
    public class VendorWithSpec : BaseSpecification<Vendor>
    {
        public VendorWithSpec(string searchByName) : base(c => c.VndNam.ToLower().Trim().Contains(searchByName.ToLower().Trim()))
        {

        }
        public VendorWithSpec(int id) : base(c => c.VndCod == id)
        {

        }
    }
}
