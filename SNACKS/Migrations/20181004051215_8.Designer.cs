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
    [Migration("20181004051215_8")]
    partial class _8
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SNACKS.Models.IngresoInsumo", b =>
                {
                    b.Property<int>("IdIngresoInsumo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdEmpleado");

                    b.HasKey("IdIngresoInsumo");

                    b.HasIndex("IdEmpleado");

                    b.ToTable("IngresoInsumo");
                });

            modelBuilder.Entity("SNACKS.Models.ItemPedido", b =>
                {
                    b.Property<int>("IdItemPedido")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Factor");

                    b.Property<int?>("IdPedido");

                    b.Property<int?>("IdProducto");

                    b.Property<int?>("IdUnidad");

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

            modelBuilder.Entity("SNACKS.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdCliente");

                    b.Property<int?>("IdUsuario");

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

                    b.Property<string>("Nombres");

                    b.Property<string>("NumeroDocumento");

                    b.Property<string>("RazonSocial");

                    b.Property<string>("TipoDocumento");

                    b.Property<int>("TipoEmpleado");

                    b.HasKey("IdPersona");

                    b.ToTable("Persona");
                });

            modelBuilder.Entity("SNACKS.Models.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EsInsumo");

                    b.Property<string>("Nombre");

                    b.HasKey("IdProducto");

                    b.ToTable("Producto");
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

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("SNACKS.Models.IngresoInsumo", b =>
                {
                    b.HasOne("SNACKS.Models.Persona", "Empleado")
                        .WithMany()
                        .HasForeignKey("IdEmpleado");
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
                        .WithMany()
                        .HasForeignKey("IdProducto");

                    b.HasOne("SNACKS.Models.Unidad", "Unidad")
                        .WithMany("ItemsProducto")
                        .HasForeignKey("IdUnidad");
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
#pragma warning restore 612, 618
        }
    }
}
