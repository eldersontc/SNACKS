using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemSalidaProductoMap : ClassMap<ItemSalidaProducto>
    {
        public ItemSalidaProductoMap()
        {
            Id(x => x.IdItemSalidaProducto);
            Map(x => x.IdSalidaProducto);
            Map(x => x.Factor);
            Map(x => x.Cantidad);
            References(x => x.Producto).Column("IdProducto");
            References(x => x.Unidad).Column("IdUnidad");
        }
    }
}
