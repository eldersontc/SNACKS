using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class InventarioProducto
    {
        [Key]
        public int IdInventarioProducto { get; set; }
        public int IdProducto { get; set; }
        public int Stock { get; set; }
        public int IdAlmacen { get; set; }
    }
}
