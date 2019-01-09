using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using mrRemoteForKodi_Update_1.Models;

namespace mrRemoteForKodi_Update_1.Migrations.Movie
{
    [DbContext(typeof(MovieContext))]
    [Migration("20170101235849_MovieMigrationUpdate4")]
    partial class MovieMigrationUpdate4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("mrRemoteForKodi_Update_1.Models.Movie", b =>
                {
                    b.Property<string>("Movieid");

                    b.Property<string>("DateAdded");

                    b.Property<string>("Director");

                    b.Property<string>("Fanart");

                    b.Property<string>("File");

                    b.Property<string>("Genre");

                    b.Property<string>("ImdbNumber");

                    b.Property<string>("Plot");

                    b.Property<string>("Poster");

                    b.Property<string>("Rating");

                    b.Property<string>("Runtime");

                    b.Property<string>("Tagline");

                    b.Property<string>("Title");

                    b.Property<string>("Writer");

                    b.Property<string>("Year");

                    b.HasKey("Movieid");

                    b.ToTable("Movies");
                });
        }
    }
}
