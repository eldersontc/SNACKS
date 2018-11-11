using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class InsumoProductoMap : ClassMap<InsumoProducto>
    {
        public InsumoProductoMap()
        {
            Id(x => x.IdInsumoProducto);
            Map(x => x.IdProducto);
            References(x => x.Insumo).Column("IdInsumo");
        }
    }
}
