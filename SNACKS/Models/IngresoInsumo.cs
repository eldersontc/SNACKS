using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class IngresoInsumo
    {
        public int IdIngresoInsumo { get; set; }
        public Usuario Usuario { get; set; }
        public Almacen Almacen { get; set; }
        public Caja Caja { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public decimal Costo { get; set; }
        public List<ItemIngresoInsumo> Items { get; set; }

        public IngresoInsumo()
        {
            Items = new List<ItemIngresoInsumo>();
        }
    }
}
