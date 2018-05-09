using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ba_Vofpffs.Migrations
{
    public partial class AddSecondTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntry");

            migrationBuilder.CreateTable(
                name: "FileEntryA",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    File = table.Column<byte[]>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    File = table.Column<byte[]>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "FileEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    File = table.Column<byte[]>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntry", x => x.ID);
                });
        }
    }
}
