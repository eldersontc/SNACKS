using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class IngresoProducto
    {
        public int IdIngresoProducto { get; set; }
        public int IdLote { get; set; }
        public Usuario Usuario { get; set; }
        public Almacen Almacen { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public IList<ItemIngresoProducto> Items { get; set; }

        public IngresoProducto()
        {
            Items = new List<ItemIngresoProducto>();
        }
    }
}
