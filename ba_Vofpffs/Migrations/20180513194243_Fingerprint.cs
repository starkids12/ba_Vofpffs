using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ba_Vofpffs.Migrations
{
    public partial class Fingerprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderFingerprint",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderFingerprint",
                table: "FileEntryA",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderFingerprint",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "HeaderFingerprint",
                table: "FileEntryA");
        }
    }
}
