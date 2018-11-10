using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class SalidaInsumoMap : ClassMap<SalidaInsumo>
    {
        public SalidaInsumoMap()
        {
            Id(x => x.IdSalidaInsumo);
            Map(x => x.IdLote);
            Map(x => x.FechaCreacion);
            Map(x => x.Comentario);
            References(x => x.Usuario).Column("IdUsuario");
            References(x => x.Almacen).Column("IdAlmacen");
        }
    }
}
