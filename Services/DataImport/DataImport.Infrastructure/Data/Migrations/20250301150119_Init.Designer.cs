﻿// <auto-generated />
using System;
using DataImport.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataImport.Infrastructure.Data.Migrations
{
    [DbContext(typeof(PricePredictContext))]
    [Migration("20250301150119_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataImport.Core.Entities.Candlestick", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("ClosePrice")
                        .HasColumnType("decimal(18,8)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<decimal>("HighPrice")
                        .HasColumnType("decimal(18,8)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<decimal>("LowPrice")
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("OpenPrice")
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Timeframe")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,8)");

                    b.HasKey("Id");

                    b.HasIndex("Symbol", "Timeframe", "Timestamp")
                        .IsUnique();

                    b.ToTable("Candlesticks");
                });
#pragma warning restore 612, 618
        }
    }
}
