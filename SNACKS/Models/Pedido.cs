﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public Persona Cliente { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaPropuesta { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal Total { get; set; }
        public decimal Pago { get; set; }
        public string Estado { get; set; }
        public List<ItemPedido> Items { get; set; }

        public Pedido()
        {
            Items = new List<ItemPedido>();
        }
    }
}
