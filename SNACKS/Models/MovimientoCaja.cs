using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class MovimientoCaja
    {
        public int IdMovimientoCaja { get; set; }
        public int IdCaja { get; set; }
        public string TipoMovimiento { get; set; }
        public int? IdCajaOrigen { get; set; }
        public int? IdPedido { get; set; }
        public int? IdIngresoInsumo { get; set; }
        public string Glosa { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Importe { get; set; }
        public Usuario Usuario { get; set; }
    }
}
