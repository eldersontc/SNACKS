using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemPedidoMap : ClassMap<ItemPedido>
    {
        public ItemPedidoMap()
        {
            Id(x => x.IdItemPedido);
            Map(x => x.IdPedido);
            Map(x => x.Factor);
            Map(x => x.Cantidad);
            Map(x => x.Total);
            References(x => x.Producto).Column("IdProducto");
            References(x => x.Unidad).Column("IdUnidad");
        }
    }
}
