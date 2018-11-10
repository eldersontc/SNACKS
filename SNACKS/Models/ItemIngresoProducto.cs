using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemIngresoProducto
    {
        public int IdItemIngresoProducto { get; set; }
        public int IdIngresoProducto { get; set; }
        public Producto Producto { get; set; }
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
    }
}
