using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }
        public int TipoPersona { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string RazonSocial { get; set; }
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Direccion { get; set; }
        [JsonIgnore]
        public List<Pedido> Pedidos { get; set; }
        [JsonIgnore]
        public List<Usuario> Usuarios { get; set; }
    }
}
