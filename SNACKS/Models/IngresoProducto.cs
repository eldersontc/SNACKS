using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class IngresoProducto
    {
        [Key]
        public int IdIngresoProducto { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public List<ItemIngresoProducto> Items { get; set; }
    }
}
