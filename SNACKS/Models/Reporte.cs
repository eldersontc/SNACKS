using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Reporte
    {
        [Key]
        public int IdReporte { get; set; }
        public string Titulo { get; set; }
        public int TipoReporte { get; set; }
        public int Flag { get; set; }
        public List<ItemReporte> Items { get; set; }
    }
}
