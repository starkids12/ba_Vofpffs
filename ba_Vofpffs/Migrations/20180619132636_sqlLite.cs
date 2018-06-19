using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ba_Vofpffs.Migrations
{
    public partial class sqlLite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileEntryA",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    Filepath = table.Column<string>(nullable: true),
                    HeaderFingerprint = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Isp = table.Column<string>(nullable: true),
                    Lat = table.Column<float>(nullable: false),
                    Lon = table.Column<float>(nullable: false),
                    RegionName = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntryA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FileEntryB",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    Filepath = table.Column<string>(nullable: true),
                    HeaderFingerprint = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Isp = table.Column<string>(nullable: true),
                    Lat = table.Column<float>(nullable: false),
                    Lon = table.Column<float>(nullable: false),
                    RegionName = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntryB", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntryA");

            migrationBuilder.DropTable(
                name: "FileEntryB");
        }
    }
}
