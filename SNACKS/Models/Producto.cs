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
        public virtual int IdProducto { get; set; }
        public virtual string Nombre { get; set; }
        public virtual bool EsInsumo { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual IList<ItemProducto> Items { get; set; }

        public Producto()
        {
            Items = new List<ItemProducto>();
        }
    }
}
