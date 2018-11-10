using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class SalidaInsumo
    {
        public int IdSalidaInsumo { get; set; }
        public int IdLote { get; set; }
        public Usuario Usuario { get; set; }
        public Almacen Almacen { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public IList<ItemSalidaInsumo> Items { get; set; }

        public SalidaInsumo()
        {
            Items = new List<ItemSalidaInsumo>();
        }
    }
}
