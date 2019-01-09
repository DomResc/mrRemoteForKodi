﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using mrRemoteForKodi_Update_1.Models;

namespace mrRemoteForKodi_Update_1.Migrations.Artist
{
    [DbContext(typeof(ArtistContext))]
    partial class ArtistContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("mrRemoteForKodi_Update_1.Models.Artist", b =>
                {
                    b.Property<string>("ArtistId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Fanart");

                    b.Property<string>("Genre");

                    b.Property<string>("Label");

                    b.Property<string>("Poster");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");
                });
        }
    }
}
