using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class InventarioProductoMap : ClassMap<InventarioProducto>
    {
        public InventarioProductoMap()
        {
            Id(x => x.IdInventarioProducto);
            Map(x => x.IdAlmacen);
            Map(x => x.IdProducto);
            Map(x => x.Stock);
        }
    }
}
