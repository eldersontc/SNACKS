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
    [Migration("20181003150258_6")]
    partial class _6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SNACKS.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Direccion");

                    b.Property<int?>("IdVendedor");

                    b.Property<string>("RazonSocial");

                    b.Property<string>("Ruc");

                    b.HasKey("IdCliente");

                    b.HasIndex("IdVendedor");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("SNACKS.Models.Empleado", b =>
                {
                    b.Property<int>("IdEmpleado")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellidos");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Nombres");

                    b.Property<string>("NumeroDocumento");

                    b.Property<string>("TipoDocumento");

                    b.HasKey("IdEmpleado");

                    b.ToTable("Empleado");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Empleado");
                });

            modelBuilder.Entity("SNACKS.Models.ItemPedido", b =>
                {
                    b.Property<int>("IdItemPedido")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IdPedido");

                    b.HasKey("IdItemPedido");

                    b.HasIndex("IdPedido");

                    b.ToTable("ItemPedido");
                });

            modelBuilder.Entity("SNACKS.Models.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<int?>("IdCliente");

                    b.HasKey("IdPedido");

                    b.HasIndex("IdCliente");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("SNACKS.Models.Vendedor", b =>
                {
                    b.HasBaseType("SNACKS.Models.Empleado");


                    b.ToTable("Vendedor");

                    b.HasDiscriminator().HasValue("Vendedor");
                });

            modelBuilder.Entity("SNACKS.Models.Cliente", b =>
                {
                    b.HasOne("SNACKS.Models.Vendedor", "Vendedor")
                        .WithMany("Clientes")
                        .HasForeignKey("IdVendedor");
                });

            modelBuilder.Entity("SNACKS.Models.ItemPedido", b =>
                {
                    b.HasOne("SNACKS.Models.Pedido", "Pedido")
                        .WithMany("Items")
                        .HasForeignKey("IdPedido");
                });

            modelBuilder.Entity("SNACKS.Models.Pedido", b =>
                {
                    b.HasOne("SNACKS.Models.Cliente", "Cliente")
                        .WithMany("pedidos")
                        .HasForeignKey("IdCliente");
                });
#pragma warning restore 612, 618
        }
    }
}
