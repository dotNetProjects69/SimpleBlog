﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SimpleBlog.Data.Context;

#nullable disable

namespace SimpleBlog.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240725150135_AddPasswordColumnToAccountsTable")]
    partial class AddPasswordColumnToAccountsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SimpleBlog.Data.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LikeId"));

                    b.Property<int>("AccountReceiverId")
                        .HasColumnType("integer");

                    b.Property<int>("AccountSenderId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LikeReceivedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PostReceiverId")
                        .HasColumnType("integer");

                    b.HasKey("LikeId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<int>("AccountOwnerAccountId")
                        .HasColumnType("integer");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("PostId");

                    b.HasIndex("AccountOwnerAccountId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.PostLike", b =>
                {
                    b.Property<int>("PostLikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostLikeId"));

                    b.Property<int>("LikeId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.HasKey("PostLikeId");

                    b.HasIndex("LikeId");

                    b.HasIndex("PostId");

                    b.ToTable("PostLikes");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.Post", b =>
                {
                    b.HasOne("SimpleBlog.Data.Entities.Account", "AccountOwner")
                        .WithMany("Posts")
                        .HasForeignKey("AccountOwnerAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountOwner");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.PostLike", b =>
                {
                    b.HasOne("SimpleBlog.Data.Entities.Like", "Like")
                        .WithMany("PostLikes")
                        .HasForeignKey("LikeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleBlog.Data.Entities.Post", "Post")
                        .WithMany("PostLikes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Like");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.Account", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.Like", b =>
                {
                    b.Navigation("PostLikes");
                });

            modelBuilder.Entity("SimpleBlog.Data.Entities.Post", b =>
                {
                    b.Navigation("PostLikes");
                });
#pragma warning restore 612, 618
        }
    }
}
