using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.ItemSpecifications
{
    public class ItemWithSpec : BaseSpecification<Items>
    {
        public ItemWithSpec(string searchByName) : base(c => c.ItmNam.ToLower().Trim().Contains(searchByName.ToLower().Trim()))
        {

        }
    }
}
