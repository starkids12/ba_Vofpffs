using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ba_Vofpffs.Migrations
{
    public partial class filePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "FileEntryB",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Filepath",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AlterColumn<long> (
                name: "Size",
                table: "FileEntryA",
                nullable: false,
                oldClrType: typeof (int));

            migrationBuilder.AddColumn<string> (
                name: "Filepath",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.DropColumn (
                name: "File",
                table: "FileEntryA");

            migrationBuilder.DropColumn (
                name: "File",
                table: "FileEntryB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]> (
                name: "File",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]> (
                name: "File",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.AlterColumn<int> (
                name: "Size",
                table: "FileEntryA",
                nullable: false,
                oldClrType: typeof (long));

            migrationBuilder.AlterColumn<int> (
                name: "Size",
                table: "FileEntryB",
                nullable: false,
                oldClrType: typeof (long));

            migrationBuilder.DropColumn (
                name: "Filepath",
                table: "FileEntryA");

            migrationBuilder.DropColumn (
                name: "Filepath",
                table: "FileEntryB");
        }
    }
}
