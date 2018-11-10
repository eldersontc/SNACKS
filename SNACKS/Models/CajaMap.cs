using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class CajaMap : ClassMap<Caja>
    {
        public CajaMap()
        {
            Id(x => x.IdCaja);
            Map(x => x.Nombre);
            Map(x => x.Saldo);
        }
    }
}
