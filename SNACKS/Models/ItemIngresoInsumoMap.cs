using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ItemIngresoInsumoMap : ClassMap<ItemIngresoInsumo>
    {
        public ItemIngresoInsumoMap()
        {
            Id(x => x.IdItemIngresoInsumo);
            Map(x => x.IdIngresoInsumo);
            Map(x => x.Factor);
            Map(x => x.Cantidad);
            Map(x => x.Costo);
            References(x => x.Unidad).Column("IdUnidad");
            References(x => x.Producto).Column("IdProducto");
        }
    }
}
