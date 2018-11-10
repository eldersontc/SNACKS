﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SNACKS.Data;

namespace SNACKS.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181102074753_cajas")]
    partial class cajas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SNACKS.Models.Almacen", b =>
                {
                    b.Property<int>("IdAlmacen")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("IdAlmacen");

                    b.ToTable("Almacen");
                });

            modelBuilder.Entity("SNACKS.Models.Caja", b =>
                {
                    b.Property<int>("IdCaja")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.Property<decimal>("Saldo");

                    b.HasKey("IdCaja");

                    b.ToTable("Caja");
                });

            modelBuilder.Entity("SNACKS.Models.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("IdCategoria");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("SNACKS.Models.IngresoInsumo", b =>
                {
                    b.Property<int>("IdIngresoInsumo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario");

                    b.Property<decimal>("Costo");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdAlmacen");

                    b.Property<int?>("IdUsuario");

                    b.HasKey("IdIngresoInsumo");

                    b.HasIndex("IdAlmacen");

                    b.HasIndex("IdUsuario");

                    b.ToTable("IngresoInsumo");
                });

            modelBuilder.Entity("SNACKS.Models.IngresoProducto", b =>
                {
                    b.Property<int>("IdIngresoProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdAlmacen");

                    b.Property<int?>("IdUsuario");

                    b.HasKey("IdIngresoProducto");

                    b.HasIndex("IdAlmacen");

                    b.HasIndex("IdUsuario");

                    b.ToTable("IngresoProducto");
                });

            modelBuilder.Entity("SNACKS.Models.InventarioInsumo", b =>
                {
                    b.Property<int>("IdInventarioInsumo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdAlmacen");

                    b.Property<int>("IdInsumo");

                    b.Property<int>("Stock");

                    b.HasKey("IdInventarioInsumo");

                    b.ToTable("InventarioInsumo");
                });

            modelBuilder.Entity("SNACKS.Models.InventarioProducto", b =>
                {
                    b.Property<int>("IdInventarioProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdAlmacen");

                    b.Property<int>("IdProducto");

                    b.Property<int>("Stock");

                    b.HasKey("IdInventarioProducto");

                    b.ToTable("InventarioProducto");
                });

            modelBuilder.Entity("SNACKS.Models.ItemIngresoInsumo", b =>
                {
                    b.Property<int>("IdItemIngresoInsumo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<decimal>("Costo");

                    b.Property<int>("Factor");

                    b.Property<int?>("IdIngresoInsumo");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdUnidad");

                    b.HasKey("IdItemIngresoInsumo");

                    b.HasIndex("IdIngresoInsumo");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdUnidad");

                    b.ToTable("ItemIngresoInsumo");
                });

            modelBuilder.Entity("SNACKS.Models.ItemIngresoProducto", b =>
                {
                    b.Property<int>("IdItemIngresoProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<int>("Factor");

                    b.Property<int?>("IdIngresoProducto");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdUnidad");

                    b.HasKey("IdItemIngresoProducto");

                    b.HasIndex("IdIngresoProducto");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdUnidad");

                    b.ToTable("ItemIngresoProducto");
                });

            modelBuilder.Entity("SNACKS.Models.ItemPedido", b =>
                {
                    b.Property<int>("IdItemPedido")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<int>("Factor");

                    b.Property<int?>("IdPedido");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdUnidad");

                    b.Property<decimal>("Total");

                    b.HasKey("IdItemPedido");

                    b.HasIndex("IdPedido");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdUnidad");

                    b.ToTable("ItemPedido");
                });

            modelBuilder.Entity("SNACKS.Models.ItemProducto", b =>
                {
                    b.Property<int>("IdItemProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Factor");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdUnidad");

                    b.HasKey("IdItemProducto");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdUnidad");

                    b.ToTable("ItemProducto");
                });

            modelBuilder.Entity("SNACKS.Models.ItemReporte", b =>
                {
                    b.Property<int>("IdItemReporte")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IdReporte");

                    b.Property<string>("Nombre");

                    b.Property<string>("Valor");

                    b.HasKey("IdItemReporte");

                    b.HasIndex("IdReporte");

                    b.ToTable("ItemReporte");
                });

            modelBuilder.Entity("SNACKS.Models.ItemSalidaInsumo", b =>
                {
                    b.Property<int>("IdItemSalidaInsumo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<int>("Factor");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdSalidaInsumo");

                    b.Property<int?>("IdUnidad");

                    b.HasKey("IdItemSalidaInsumo");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdSalidaInsumo");

                    b.HasIndex("IdUnidad");

                    b.ToTable("ItemSalidaInsumo");
                });

            modelBuilder.Entity("SNACKS.Models.ItemSalidaProducto", b =>
                {
                    b.Property<int>("IdItemSalidaProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<int>("Factor");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdSalidaProducto");

                    b.Property<int?>("IdUnidad");

                    b.HasKey("IdItemSalidaProducto");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdSalidaProducto");

                    b.HasIndex("IdUnidad");

                    b.ToTable("ItemSalidaProducto");
                });

            modelBuilder.Entity("SNACKS.Models.MovimientoCaja", b =>
                {
                    b.Property<int>("IdMovimientoCaja")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Glosa");

                    b.Property<int?>("IdCaja");

                    b.Property<int?>("IdCajaOrigen");

                    b.Property<int?>("IdIngresoInsumo");

                    b.Property<int?>("IdPedido");

                    b.Property<decimal>("Importe");

                    b.Property<string>("TipoMovimiento");

                    b.HasKey("IdMovimientoCaja");

                    b.HasIndex("IdCaja");

                    b.ToTable("MovimientoCaja");
                });

            modelBuilder.Entity("SNACKS.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario");

                    b.Property<string>("Estado");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<DateTime?>("FechaEntrega");

                    b.Property<DateTime?>("FechaPago");

                    b.Property<DateTime>("FechaPropuesta");

                    b.Property<int?>("IdCliente");

                    b.Property<int?>("IdUsuario");

                    b.Property<decimal>("Pago");

                    b.Property<decimal>("Total");

                    b.HasKey("IdPedido");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("SNACKS.Models.Persona", b =>
                {
                    b.Property<int>("IdPersona")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellidos");

                    b.Property<string>("Direccion");

                    b.Property<string>("Distrito");

                    b.Property<int?>("IdVendedor");

                    b.Property<int?>("IdZonaVenta");

                    b.Property<string>("Nombres");

                    b.Property<string>("NumeroDocumento");

                    b.Property<string>("RazonSocial");

                    b.Property<int>("TipoDocumento");

                    b.Property<int>("TipoPersona");

                    b.HasKey("IdPersona");

                    b.HasIndex("IdVendedor");

                    b.HasIndex("IdZonaVenta");

                    b.ToTable("Persona");
                });

            modelBuilder.Entity("SNACKS.Models.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EsInsumo");

                    b.Property<int?>("IdCategoria");

                    b.Property<string>("Nombre");

                    b.HasKey("IdProducto");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Producto");
                });

            modelBuilder.Entity("SNACKS.Models.Reporte", b =>
                {
                    b.Property<int>("IdReporte")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Flag");

                    b.Property<string>("TipoReporte");

                    b.Property<string>("Titulo");

                    b.HasKey("IdReporte");

                    b.ToTable("Reporte");
                });

            modelBuilder.Entity("SNACKS.Models.SalidaInsumo", b =>
                {
                    b.Property<int>("IdSalidaInsumo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdAlmacen");

                    b.Property<int?>("IdUsuario");

                    b.HasKey("IdSalidaInsumo");

                    b.HasIndex("IdAlmacen");

                    b.HasIndex("IdUsuario");

                    b.ToTable("SalidaInsumo");
                });

            modelBuilder.Entity("SNACKS.Models.SalidaProducto", b =>
                {
                    b.Property<int>("IdSalidaProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdAlmacen");

                    b.Property<int?>("IdUsuario");

                    b.HasKey("IdSalidaProducto");

                    b.HasIndex("IdAlmacen");

                    b.HasIndex("IdUsuario");

                    b.ToTable("SalidaProducto");
                });

            modelBuilder.Entity("SNACKS.Models.Unidad", b =>
                {
                    b.Property<int>("IdUnidad")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abreviacion");

                    b.Property<string>("Nombre");

                    b.HasKey("IdUnidad");

                    b.ToTable("Unidad");
                });

            modelBuilder.Entity("SNACKS.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Clave");

                    b.Property<int?>("IdPersona");

                    b.Property<string>("Nombre");

                    b.HasKey("IdUsuario");

                    b.HasIndex("IdPersona");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("SNACKS.Models.ZonaVenta", b =>
                {
                    b.Property<int>("IdZonaVenta")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("IdZonaVenta");

                    b.ToTable("ZonaVenta");
                });

            modelBuilder.Entity("SNACKS.Models.IngresoInsumo", b =>
                {
                    b.HasOne("SNACKS.Models.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("IdAlmacen");

                    b.HasOne("SNACKS.Models.Usuario", "Usuario")
                        .WithMany("IntresosInsumo")
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SNACKS.Models.IngresoProducto", b =>
                {
                    b.HasOne("SNACKS.Models.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("IdAlmacen");

                    b.HasOne("SNACKS.Models.Usuario", "Usuario")
                        .WithMany("IntresosProducto")
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SNACKS.Models.ItemIngresoInsumo", b =>
                {
                    b.HasOne("SNACKS.Models.IngresoInsumo", "IngresoInsumo")
                        .WithMany("Items")
                        .HasForeignKey("IdIngresoInsumo");

                    b.HasOne("SNACKS.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany()
                        .HasForeignKey("IdUnidad");
                });

            modelBuilder.Entity("SNACKS.Models.ItemIngresoProducto", b =>
                {
                    b.HasOne("SNACKS.Models.IngresoProducto", "IngresoProducto")
                        .WithMany("Items")
                        .HasForeignKey("IdIngresoProducto");

                    b.HasOne("SNACKS.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany()
                        .HasForeignKey("IdUnidad");
                });

            modelBuilder.Entity("SNACKS.Models.ItemPedido", b =>
                {
                    b.HasOne("SNACKS.Models.Pedido", "Pedido")
                        .WithMany("Items")
                        .HasForeignKey("IdPedido");

                    b.HasOne("SNACKS.Models.Producto", "Producto")
                        .WithMany("ItemsPedido")
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany("ItemsPedido")
                        .HasForeignKey("IdUnidad");
                });

            modelBuilder.Entity("SNACKS.Models.ItemProducto", b =>
                {
                    b.HasOne("SNACKS.Models.Producto", "Producto")
                        .WithMany("Items")
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany("ItemsProducto")
                        .HasForeignKey("IdUnidad");
                });

            modelBuilder.Entity("SNACKS.Models.ItemReporte", b =>
                {
                    b.HasOne("SNACKS.Models.Reporte", "Reporte")
                        .WithMany("Items")
                        .HasForeignKey("IdReporte");
                });

            modelBuilder.Entity("SNACKS.Models.ItemSalidaInsumo", b =>
                {
                    b.HasOne("SNACKS.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.SalidaInsumo", "SalidaInsumo")
                        .WithMany("Items")
                        .HasForeignKey("IdSalidaInsumo");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany()
                        .HasForeignKey("IdUnidad");
                });

            modelBuilder.Entity("SNACKS.Models.ItemSalidaProducto", b =>
                {
                    b.HasOne("SNACKS.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.SalidaProducto", "SalidaProducto")
                        .WithMany("Items")
                        .HasForeignKey("IdSalidaProducto");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany()
                        .HasForeignKey("IdUnidad");
                });

            modelBuilder.Entity("SNACKS.Models.MovimientoCaja", b =>
                {
                    b.HasOne("SNACKS.Models.Caja", "Caja")
                        .WithMany("Movimientos")
                        .HasForeignKey("IdCaja");
                });

            modelBuilder.Entity("SNACKS.Models.Pedido", b =>
                {
                    b.HasOne("SNACKS.Models.Persona", "Cliente")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdCliente");

                    b.HasOne("SNACKS.Models.Usuario", "Usuario")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SNACKS.Models.Persona", b =>
                {
                    b.HasOne("SNACKS.Models.Persona", "Vendedor")
                        .WithMany()
                        .HasForeignKey("IdVendedor");

                    b.HasOne("SNACKS.Models.ZonaVenta", "ZonaVenta")
                        .WithMany("Personas")
                        .HasForeignKey("IdZonaVenta");
                });

            modelBuilder.Entity("SNACKS.Models.Producto", b =>
                {
                    b.HasOne("SNACKS.Models.Categoria", "Categoria")
                        .WithMany("Productos")
                        .HasForeignKey("IdCategoria");
                });

            modelBuilder.Entity("SNACKS.Models.SalidaInsumo", b =>
                {
                    b.HasOne("SNACKS.Models.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("IdAlmacen");

                    b.HasOne("SNACKS.Models.Usuario", "Usuario")
                        .WithMany("SalidasInsumo")
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SNACKS.Models.SalidaProducto", b =>
                {
                    b.HasOne("SNACKS.Models.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("IdAlmacen");

                    b.HasOne("SNACKS.Models.Usuario", "Usuario")
                        .WithMany("SalidasProducto")
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SNACKS.Models.Usuario", b =>
                {
                    b.HasOne("SNACKS.Models.Persona", "Persona")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdPersona");
                });
#pragma warning restore 612, 618
        }
    }
}
