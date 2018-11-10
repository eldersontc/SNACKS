using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Id(x => x.IdUsuario);
            Map(x => x.Nombre);
            Map(x => x.Clave);
            References(x => x.Persona).Column("IdPersona");
        }
    }
}
