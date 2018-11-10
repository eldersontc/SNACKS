using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Lote
    {
        public int IdLote { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Usuario { get; set; }
        public IList<ItemLote> Items { get; set; }

        public Lote() => Items = new List<ItemLote>();
    }
}
