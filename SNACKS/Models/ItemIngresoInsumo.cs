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
        public int IdItemIngresoInsumo { get; set; }
        public int IdIngresoInsumo { get; set; }
        public Producto Producto { get; set; }
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
        public decimal Costo { get; set; }
    }
}
