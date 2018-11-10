using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Categoria
    {
        public virtual int IdCategoria { get; set; }
        public virtual string Nombre { get; set; }
    }
}
