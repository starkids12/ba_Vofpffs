﻿// <auto-generated />
using ba_Vofpffs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ba_Vofpffs.Migrations
{
    [DbContext(typeof(FileEntryContext))]
    partial class FileEntryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ba_Vofpffs.Models.FileEntryItemA", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Filename");

                    b.Property<string>("Filepath");

                    b.Property<string>("HeaderFingerprint");

                    b.Property<string>("Headers");

                    b.Property<string>("IPAddress");

                    b.Property<string>("Isp");

                    b.Property<float>("Lat");

                    b.Property<float>("Lon");

                    b.Property<string>("RegionName");

                    b.Property<long>("Size");

                    b.HasKey("ID");

                    b.ToTable("FileEntryA");
                });

            modelBuilder.Entity("ba_Vofpffs.Models.FileEntryItemB", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Filename");

                    b.Property<string>("Filepath");

                    b.Property<string>("HeaderFingerprint");

                    b.Property<string>("Headers");

                    b.Property<string>("IPAddress");

                    b.Property<string>("Isp");

                    b.Property<float>("Lat");

                    b.Property<float>("Lon");

                    b.Property<string>("RegionName");

                    b.Property<long>("Size");

                    b.HasKey("ID");

                    b.ToTable("FileEntryB");
                });
#pragma warning restore 612, 618
        }
    }
}
