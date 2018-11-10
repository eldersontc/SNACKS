using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class PersonaMap : ClassMap<Persona>
    {
        public PersonaMap()
        {
            Id(x => x.IdPersona);
            Map(x => x.TipoPersona);
            Map(x => x.Nombres);
            Map(x => x.Apellidos);
            Map(x => x.RazonSocial);
            Map(x => x.TipoDocumento);
            Map(x => x.NumeroDocumento);
            Map(x => x.Distrito);
            Map(x => x.Direccion);
            References(x => x.ZonaVenta).Column("IdZonaVenta");
            References(x => x.Vendedor).Column("IdVendedor");
        }
    }
}
