using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemIngresoInsumo
    {
        [Key]
        public int IdItemIngresoInsumo { get; set; }
        [ForeignKey("IdIngresoInsumo")]
        public IngresoInsumo IngresoInsumo { get; set; }
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
        [ForeignKey("IdUnidad")]
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
        public decimal Costo { get; set; }
    }
}
