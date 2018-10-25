using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class InventarioInsumo
    {
        [Key]
        public int IdInventarioInsumo { get; set; }
        public int IdInsumo { get; set; }
        public int Stock { get; set; }
    }
}
