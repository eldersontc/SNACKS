using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ReporteMap : ClassMap<Reporte>
    {
        public ReporteMap()
        {
            Id(x => x.IdReporte);
            Map(x => x.Titulo);
            Map(x => x.TipoReporte);
            Map(x => x.Flag);
        }
    }
}
