using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ZonaVentaMap : ClassMap<ZonaVenta>
    {
        public ZonaVentaMap()
        {
            Id(x => x.IdZonaVenta);
            Map(x => x.Nombre);
        }
    }
}
