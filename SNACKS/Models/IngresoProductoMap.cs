using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class IngresoProductoMap : ClassMap<IngresoProducto>
    {
        public IngresoProductoMap()
        {
            Id(x => x.IdIngresoProducto);
            Map(x => x.IdLote);
            Map(x => x.FechaCreacion);
            Map(x => x.Comentario);
            References(x => x.Usuario).Column("IdUsuario");
            References(x => x.Almacen).Column("IdAlmacen");
        }
    }
}
