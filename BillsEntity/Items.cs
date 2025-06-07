using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsEntity
{
    public class Items
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItmCod { get; set; }
        [Required]
        public string ItmNam { get; set; }
        [Required]
        public decimal ItmPrc { get; set; }
    }
}
