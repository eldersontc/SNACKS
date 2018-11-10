using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class LoteMap : ClassMap<Lote>
    {
        public LoteMap()
        {
            Id(x => x.IdLote);
            Map(x => x.Fecha);
            References(x => x.Usuario).Column("IdUsuario");
        }
    }
}
