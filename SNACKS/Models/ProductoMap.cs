using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ProductoMap : ClassMap<Producto>
    {
        public ProductoMap()
        {
            Id(x => x.IdProducto);
            Map(x => x.Nombre);
            Map(x => x.EsInsumo);
            References(x => x.Categoria).Column("IdCategoria");
        }
    }
}
