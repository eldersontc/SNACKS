using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class UnidadMap : ClassMap<Unidad>
    {
        public UnidadMap()
        {
            Id(x => x.IdUnidad);
            Map(x => x.Nombre);
            Map(x => x.Abreviacion);
        }
    }
}
