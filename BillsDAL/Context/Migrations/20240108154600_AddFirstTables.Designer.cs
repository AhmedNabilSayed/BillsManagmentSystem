﻿// <auto-generated />
using System;
using BillsDAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BillsDAL.Migrations
{
    [DbContext(typeof(BillsDbContext))]
    [Migration("20240108154600_AddFirstTables")]
    partial class AddFirstTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BillsEntity.BillDetails", b =>
                {
                    b.Property<int>("DTLCOD")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DTLCOD"), 1L, 1);

                    b.Property<int>("BILCOD")
                        .HasColumnType("int");

                    b.Property<int>("ITMCOD")
                        .HasColumnType("int");

                    b.Property<decimal>("ITMPRC")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ITMQTY")
                        .HasColumnType("int");

                    b.HasKey("DTLCOD");

                    b.HasIndex("BILCOD");

                    b.HasIndex("ITMCOD");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("BillsEntity.BillHeader", b =>
                {
                    b.Property<int>("BILCOD")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BILCOD"), 1L, 1);

                    b.Property<DateTime>("BILDAT")
                        .HasColumnType("datetime2");

                    b.Property<string>("BILIMG")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("BILPRC")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VNDCOD")
                        .HasColumnType("int");

                    b.HasKey("BILCOD");

                    b.HasIndex("VNDCOD");

                    b.ToTable("BillHeaders");
                });

            modelBuilder.Entity("BillsEntity.Items", b =>
                {
                    b.Property<int>("ItmCod")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItmCod"), 1L, 1);

                    b.Property<string>("ItmNam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ItmPrc")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ItmCod");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("BillsEntity.Vendor", b =>
                {
                    b.Property<int>("VndCod")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VndCod"), 1L, 1);

                    b.Property<string>("VndNam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VndCod");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("BillsEntity.BillDetails", b =>
                {
                    b.HasOne("BillsEntity.BillHeader", "BillHeader")
                        .WithMany()
                        .HasForeignKey("BILCOD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillsEntity.Items", "Item")
                        .WithMany()
                        .HasForeignKey("ITMCOD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillHeader");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("BillsEntity.BillHeader", b =>
                {
                    b.HasOne("BillsEntity.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VNDCOD")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vendor");
                });
#pragma warning restore 612, 618
        }
    }
}
