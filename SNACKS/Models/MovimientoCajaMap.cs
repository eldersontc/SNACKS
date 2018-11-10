using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class MovimientoCajaMap : ClassMap<MovimientoCaja>
    {
        public MovimientoCajaMap()
        {
            Id(x => x.IdMovimientoCaja);
            Map(x => x.IdCaja);
            Map(x => x.TipoMovimiento);
            Map(x => x.IdCajaOrigen);
            Map(x => x.IdPedido);
            Map(x => x.IdIngresoInsumo);
            Map(x => x.Glosa);
            Map(x => x.Fecha);
            Map(x => x.Importe);
        }
    }
}
