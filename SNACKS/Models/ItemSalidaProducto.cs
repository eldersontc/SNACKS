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
        [Key]
        public int IdItemSalidaProducto { get; set; }
        [ForeignKey("IdSalidaProducto")]
        public SalidaProducto SalidaProducto { get; set; }
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
        [ForeignKey("IdUnidad")]
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
    }
}
