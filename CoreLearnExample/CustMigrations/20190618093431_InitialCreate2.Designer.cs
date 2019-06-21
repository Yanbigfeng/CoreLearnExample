﻿// <auto-generated />
using System;
using CoreLearnExample.EData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreLearnExample.CustMigrations
{
    [DbContext(typeof(testContext))]
    [Migration("20190618093431_InitialCreate2")]
    partial class InitialCreate2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreLearnExample.EData.TableTest", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Describe")
                        .HasMaxLength(50);

                    b.Property<string>("Introduce")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("TableTest");
                });
#pragma warning restore 612, 618
        }
    }
}
