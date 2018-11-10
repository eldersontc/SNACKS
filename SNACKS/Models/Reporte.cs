using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Reporte
    {
        public virtual int IdReporte { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string TipoReporte { get; set; }
        public virtual int Flag { get; set; }
        public virtual List<ItemReporte> Items { get; set; }

        public Reporte()
        {
            Items = new List<ItemReporte>();
        }
    }
}
