using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SNACKS.Models;

namespace SNACKS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Unidad> Unidad { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ItemProducto> ItemProducto { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<ItemPedido> ItemPedido { get; set; }
        public DbSet<IngresoInsumo> IngresoInsumo { get; set; }
        public DbSet<ItemIngresoInsumo> ItemIngresoInsumo { get; set; }
        public DbSet<IngresoProducto> IngresoProducto { get; set; }
        public DbSet<ItemIngresoProducto> ItemIngresoProducto { get; set; }
        public DbSet<SalidaInsumo> SalidaInsumo { get; set; }
        public DbSet<ItemSalidaInsumo> ItemSalidaInsumo { get; set; }
        public DbSet<SalidaProducto> SalidaProducto { get; set; }
        public DbSet<ItemSalidaProducto> ItemSalidaProducto { get; set; }
    }
}