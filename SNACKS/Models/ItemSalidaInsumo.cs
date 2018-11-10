using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemSalidaInsumo
    {
        public int IdItemSalidaInsumo { get; set; }
        public int IdSalidaInsumo { get; set; }
        public Producto Producto { get; set; }
        public Producto Insumo { get; set; }
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
    }
}
