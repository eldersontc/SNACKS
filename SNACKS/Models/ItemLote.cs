using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemLote
    {
        public int IdItemLote { get; set; }
        public int IdLote { get; set; }
        public Producto Producto { get; set; }
    }
}
