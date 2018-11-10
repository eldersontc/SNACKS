using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class AlmacenMap : ClassMap<Almacen>
    {
        public AlmacenMap()
        {
            Id(x => x.IdAlmacen);
            Map(x => x.Nombre);
        }
    }
}
