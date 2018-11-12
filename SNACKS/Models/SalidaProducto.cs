using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class SalidaProducto
    {
        public int IdSalidaProducto { get; set; }
        public Usuario Usuario { get; set; }
        public Almacen Almacen { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public int? IdPedido { get; set; }
        public List<ItemSalidaProducto> Items { get; set; }

        public SalidaProducto()
        {
            Items = new List<ItemSalidaProducto>();
        }
    }
}
