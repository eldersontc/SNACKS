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
        public virtual int IdItemReporte { get; set; }
        public virtual int IdReporte { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Valor { get; set; }
    }
}
