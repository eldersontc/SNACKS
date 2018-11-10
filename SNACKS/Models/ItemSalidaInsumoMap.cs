using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemSalidaInsumoMap : ClassMap<ItemSalidaInsumo>
    {
        public ItemSalidaInsumoMap()
        {
            Id(x => x.IdItemSalidaInsumo);
            Map(x => x.IdSalidaInsumo);
            Map(x => x.Factor);
            Map(x => x.Cantidad);
            References(x => x.Producto).Column("IdProducto");
            References(x => x.Insumo).Column("IdInsumo");
            References(x => x.Unidad).Column("IdUnidad");
        }
    }
}
