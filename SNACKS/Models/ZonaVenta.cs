using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class ZonaVenta
    {
        [Key]
        public int IdZonaVenta { get; set; }
        public string Nombre { get; set; }
        [JsonIgnore]
        public List<Persona> Personas { get; set; }
    }
}
