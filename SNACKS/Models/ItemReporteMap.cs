using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemReporteMap : ClassMap<ItemReporte>
    {
        public ItemReporteMap()
        {
            Id(x => x.IdItemReporte);
            Map(x => x.IdReporte);
            Map(x => x.Nombre);
            Map(x => x.Valor);
        }
    }
}
