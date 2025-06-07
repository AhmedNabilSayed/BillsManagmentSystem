using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsEntity
{
    public class BillDetails
    {
        [Key]
        public int DTLCOD { get; set; }
        [Required]
        public decimal ITMPRC { get; set; }
        [Required]
        public int ITMQTY { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
		[Required]
        public int BILCOD { get; set; }
        [ForeignKey("BILCOD")]
        public BillHeader BillHeader { get; set; }

        [Required]
        public int ITMCOD { get; set; }
        [ForeignKey("ITMCOD")]
        public Items Item { get; set; }
    }
}
