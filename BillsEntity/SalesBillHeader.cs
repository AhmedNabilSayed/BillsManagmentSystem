using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsEntity
{
    public class SalesBillHeader
    {
        [Key]
        public int BILCOD { get; set; }
        [Required]
        public DateTime BILDAT { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [Required]
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Stores Store { get; set; }
        public decimal? BILPRC { get; set; }
        public string? BILIMG { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
