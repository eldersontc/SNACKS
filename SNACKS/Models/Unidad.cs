using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Unidad
    {
        public virtual int IdUnidad { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Abreviacion { get; set; }
    }
}
