using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public bool EsProducto { get; set; }
        public bool EsInsumo { get; set; }
        public bool EsGasto { get; set; }
        public Categoria Categoria { get; set; }
        public IList<ItemProducto> Items { get; set; }
        public IList<InsumoProducto> Insumos { get; set; }

        public Producto()
        {
            Items = new List<ItemProducto>();
            Insumos = new List<InsumoProducto>();
        }
    }
}
