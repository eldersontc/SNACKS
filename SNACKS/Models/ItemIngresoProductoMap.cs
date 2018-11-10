using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemIngresoProductoMap : ClassMap<ItemIngresoProducto>
    {
        public ItemIngresoProductoMap()
        {
            Id(x => x.IdItemIngresoProducto);
            Map(x => x.IdIngresoProducto);
            Map(x => x.Factor);
            Map(x => x.Cantidad);
            References(x => x.Producto).Column("IdProducto");
            References(x => x.Unidad).Column("IdUnidad");
        }
    }
}
