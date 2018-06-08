using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ba_Vofpffs.Migrations
{
    public partial class GeoInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isp",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lon",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "FileEntryB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isp",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lon",
                table: "FileEntryA",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "FileEntryA",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "Isp",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "FileEntryB");

            migrationBuilder.DropColumn(
                name: "City",
                table: "FileEntryA");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "FileEntryA");

            migrationBuilder.DropColumn(
                name: "Isp",
                table: "FileEntryA");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "FileEntryA");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "FileEntryA");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "FileEntryA");
        }
    }
}
