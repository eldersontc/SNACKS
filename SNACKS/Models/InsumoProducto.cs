using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class InsumoProducto
    {
        public int IdInsumoProducto { get; set; }
        public int IdProducto { get; set; }
        public Producto Insumo { get; set; }
    }
}
