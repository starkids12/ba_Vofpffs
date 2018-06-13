using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ba_Vofpffs.Migrations
{
    public partial class HashToFilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "FileEntryB",
                newName: "Filename");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "FileEntryA",
                newName: "Filename");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "FileEntryB",
                newName: "Hash");

            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "FileEntryA",
                newName: "Hash");
        }
    }
}
