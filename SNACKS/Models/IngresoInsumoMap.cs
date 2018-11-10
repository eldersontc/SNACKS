using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class IngresoInsumoMap : ClassMap<IngresoInsumo>
    {
        public IngresoInsumoMap()
        {
            Id(x => x.IdIngresoInsumo);
            Map(x => x.FechaCreacion);
            Map(x => x.Comentario);
            Map(x => x.Costo);
            References(x => x.Usuario).Column("IdUsuario");
            References(x => x.Almacen).Column("IdAlmacen");
            References(x => x.Caja).Column("IdCaja");
        }
    }
}
