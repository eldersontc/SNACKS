using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class PedidoMap : ClassMap<Pedido>
    {
        public PedidoMap()
        {
            Id(x => x.IdPedido);
            Map(x => x.FechaCreacion);
            Map(x => x.Comentario);
            Map(x => x.FechaPropuesta);
            Map(x => x.FechaEntrega);
            Map(x => x.Total);
            Map(x => x.Pago);
            Map(x => x.Estado);
            References(x => x.Cliente).Column("IdCliente");
            References(x => x.Usuario).Column("IdUsuario");
        }
    }
}
