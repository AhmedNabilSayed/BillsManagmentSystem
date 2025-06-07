using BillsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications;

namespace BillsBLL.Specifications.StockSpecifications
{
    public class StockWithSpec : BaseSpecification<Stock>
    {
        public StockWithSpec()
        {
            AddInclude(i => i.Store);
            AddInclude(i => i.Item);
        }
        public StockWithSpec(int? itemId = null, int? storeId = null)
             : base(c => (!itemId.HasValue || c.ItemId == itemId) && (!storeId.HasValue || c.StoreId == storeId))
        {
            AddInclude(i => i.Store);
            AddInclude(i => i.Item);
        }
    }
}
