using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemProducto
    {
        [Key]
        public int IdItemProducto { get; set; }
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
        [ForeignKey("IdUnidad")]
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
    }
}
