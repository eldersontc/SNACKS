using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemSalidaProducto
    {
        public int IdItemSalidaProducto { get; set; }
        public int IdSalidaProducto { get; set; }
        public Producto Producto { get; set; }
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
    }
}
