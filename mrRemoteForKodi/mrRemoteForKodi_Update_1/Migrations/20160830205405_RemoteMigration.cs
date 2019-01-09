using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mrRemoteForKodi_Update_1.Migrations
{
    public partial class RemoteMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Remotes",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Fav = table.Column<bool>(nullable: false),
                    Host = table.Column<string>(nullable: true),
                    Pass = table.Column<string>(nullable: true),
                    Port = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remotes", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Remotes");
        }
    }
}
