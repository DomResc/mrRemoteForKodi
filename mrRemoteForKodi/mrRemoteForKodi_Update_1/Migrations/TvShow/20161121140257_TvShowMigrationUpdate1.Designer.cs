using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using mrRemoteForKodi_Update_1.Models;

namespace mrRemoteForKodi_Update_1.Migrations.TvShow
{
    [DbContext(typeof(TvShowContext))]
    [Migration("20161121140257_TvShowMigrationUpdate1")]
    partial class TvShowMigrationUpdate1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("mrRemoteForKodi_Update_1.Models.TvShow", b =>
                {
                    b.Property<string>("Tvshowid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Episode");

                    b.Property<string>("Fanart");

                    b.Property<string>("Genre");

                    b.Property<string>("Plot");

                    b.Property<string>("Poster");

                    b.Property<string>("Rating");

                    b.Property<string>("Season");

                    b.Property<string>("Title");

                    b.Property<string>("Year");

                    b.HasKey("Tvshowid");

                    b.ToTable("TvShows");
                });
        }
    }
}
