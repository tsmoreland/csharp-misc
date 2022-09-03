﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tsmoreland.EntityFramework.Core.Experimental.Infrastructure;

#nullable disable

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.Migrations
{
    [DbContext(typeof(PeopleDbContext))]
    partial class PeopleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("Tsmoreland.EntityFramework.Core.Experimental.Domain.Models.Person", b =>
                {
                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("last_name");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("_firstName")
                        .HasColumnType("TEXT")
                        .HasColumnName("first_name");

                    b.Property<string>("_firstname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("LastName", "Id");

                    b.ToTable("people", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
