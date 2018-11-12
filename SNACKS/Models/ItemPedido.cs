using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemPedido
    { 
        public int IdItemPedido { get; set; }
        public int IdPedido { get; set; }
        public Producto Producto { get; set; }
        public Unidad Unidad { get; set; }
        public int Factor { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
