using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class IngresoInsumo
    {
        [Key]
        public int IdIngresoInsumo { get; set; }
        [ForeignKey("IdEmpleado")]
        public Persona Empleado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
