using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mrRemoteForKodi_Update_1.Migrations
{
    public partial class RemoteMigrationUpdateWol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WolMac",
                table: "Remotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WolMask",
                table: "Remotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WolPort",
                table: "Remotes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WolMac",
                table: "Remotes");

            migrationBuilder.DropColumn(
                name: "WolMask",
                table: "Remotes");

            migrationBuilder.DropColumn(
                name: "WolPort",
                table: "Remotes");
        }
    }
}
