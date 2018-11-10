using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemProductoMap : ClassMap<ItemProducto>
    {
        public ItemProductoMap()
        {
            Id(x => x.IdItemProducto);
            Map(x => x.Factor);
            Map(x => x.IdProducto);
            References(x => x.Unidad).Column("IdUnidad");
        }
    }
}
