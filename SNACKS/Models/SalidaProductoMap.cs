using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class SalidaProductoMap : ClassMap<SalidaProducto>
    {
        public SalidaProductoMap()
        {
            Id(x => x.IdSalidaProducto);
            Map(x => x.FechaCreacion);
            Map(x => x.Comentario);
            Map(x => x.IdPedido);
            References(x => x.Almacen).Column("IdAlmacen");
            References(x => x.Usuario).Column("IdUsuario");
        }
    }
}
