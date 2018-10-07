using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Unidad
    {
        [Key]
        public int IdUnidad { get; set; }
        public string Nombre { get; set; }
        public string Abreviacion { get; set; }
        public List<ItemProducto> ItemsProducto { get; set; }
        public List<ItemPedido> ItemsPedido { get; set; }
    }
}
