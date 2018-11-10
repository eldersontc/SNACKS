using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Caja
    {
        public virtual int IdCaja { get; set; }
        public virtual string Nombre { get; set; }
        public virtual decimal Saldo { get; set; }
        public virtual List<MovimientoCaja> Movimientos { get; set; }

        public Caja()
        {
            Movimientos = new List<MovimientoCaja>();
        }
    }
}
