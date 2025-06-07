using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsEntity
{
    public class Stock
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Stores Store { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Items Item { get; set; }
        public int ItemQuantity { get; set; }
    }
}
