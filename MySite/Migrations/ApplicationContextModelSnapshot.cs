﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySite.Model;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MySite.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MySite.Model.Read", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoverPath");

                    b.Property<string>("Description");

                    b.Property<int?>("GoodreadsEditionId");

                    b.Property<int>("GoodreadsId");

                    b.Property<int>("Rating");

                    b.Property<DateTime?>("ReleaseDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Read");
                });

            modelBuilder.Entity("MySite.Model.Watched", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("PosterPath");

                    b.Property<int>("Rating");

                    b.Property<DateTime?>("ReleaseDate");

                    b.Property<string>("Title");

                    b.Property<int>("TmdbId");

                    b.HasKey("Id");

                    b.ToTable("Watched");
                });
#pragma warning restore 612, 618
        }
    }
}
