﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public List<Pedido> Pedidos { get; set; }
        [ForeignKey("IdPersona")]
        public Persona Persona { get; set; }
        public List<IngresoInsumo> IntresosInsumo { get; set; }
        public List<IngresoProducto> IntresosProducto { get; set; }
        public List<SalidaInsumo> SalidasInsumo { get; set; }
        public List<SalidaProducto> SalidasProducto { get; set; }
    }
}
