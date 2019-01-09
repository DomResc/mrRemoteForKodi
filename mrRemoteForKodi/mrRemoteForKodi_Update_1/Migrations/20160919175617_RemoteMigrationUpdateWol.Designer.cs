using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using mrRemoteForKodi_Update_1.Models;

namespace mrRemoteForKodi_Update_1.Migrations
{
    [DbContext(typeof(RemoteContext))]
    [Migration("20160919175617_RemoteMigrationUpdateWol")]
    partial class RemoteMigrationUpdateWol
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("mrRemoteForKodi_Update_1.Models.Remote", b =>
                {
                    b.Property<string>("Name");

                    b.Property<bool>("Fav");

                    b.Property<string>("Host");

                    b.Property<string>("Pass");

                    b.Property<string>("Port");

                    b.Property<string>("User");

                    b.Property<string>("WolMac");

                    b.Property<string>("WolMask");

                    b.Property<string>("WolPort");

                    b.HasKey("Name");

                    b.ToTable("Remotes");
                });
        }
    }
}
