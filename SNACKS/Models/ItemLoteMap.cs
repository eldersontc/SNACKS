using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemLoteMap : ClassMap<ItemLote>
    {
        public ItemLoteMap()
        {
            Id(x => x.IdItemLote);
            Map(x => x.IdLote);
            References(x => x.Producto).Column("IdProducto");
        }
    }
}
