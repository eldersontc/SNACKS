using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public bool EsInsumo { get; set; }
        public List<ItemPedido> ItemsPedido { get; set; }
        public List<ItemProducto> Items { get; set; }
    }
}
