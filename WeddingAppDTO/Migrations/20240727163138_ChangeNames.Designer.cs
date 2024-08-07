﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeddingAppDTO.Context;

#nullable disable

namespace WeddingApp.Migrations
{
    [DbContext(typeof(WeddingAppUserContext))]
    [Migration("20240727163138_ChangeNames")]
    partial class ChangeNames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WeddingAppDTO.DataTransferObject.PictureDto", b =>
                {
                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserDtoUserID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PicturePath");

                    b.HasIndex("UserDtoUserID");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("WeddingAppDTO.DataTransferObject.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserID");

                    b.HasIndex("UserPhone")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WeddingAppDTO.DataTransferObject.PictureDto", b =>
                {
                    b.HasOne("WeddingAppDTO.DataTransferObject.User", null)
                        .WithMany("Posts")
                        .HasForeignKey("UserDtoUserID");
                });

            modelBuilder.Entity("WeddingAppDTO.DataTransferObject.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
