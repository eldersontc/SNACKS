using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemProducto
    {
        public virtual int IdItemProducto { get; set; }
        public virtual int IdProducto { get; set; }
        public virtual Unidad Unidad { get; set; }
        public virtual int Factor { get; set; }
    }
}
