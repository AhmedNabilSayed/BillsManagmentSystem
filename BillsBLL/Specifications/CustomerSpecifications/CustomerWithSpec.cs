using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.CustomerSpecifications
{
    public class CustomerWithSpec : BaseSpecification<Customer>
    {
        public CustomerWithSpec(string searchByName) : base(c => c.CustomerName.ToLower().Trim().Contains( searchByName.ToLower().Trim()))
        {

        }
    }
}
