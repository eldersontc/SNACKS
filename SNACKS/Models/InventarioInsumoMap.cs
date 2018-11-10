using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class InventarioInsumoMap : ClassMap<InventarioInsumo>
    {
        public InventarioInsumoMap()
        {
            Id(x => x.IdInventarioInsumo);
            Map(x => x.IdAlmacen);
            Map(x => x.IdInsumo);
            Map(x => x.Stock);
        }
    }
}
