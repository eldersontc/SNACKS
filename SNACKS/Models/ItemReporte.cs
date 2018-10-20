using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemReporte
    {
        [Key]
        public int IdItemReporte { get; set; }
        [ForeignKey("IdReporte")]
        public Reporte Reporte { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
