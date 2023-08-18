﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using journalapi;

#nullable disable

namespace journalApi.Migrations
{
    [DbContext(typeof(JournalContext))]
    [Migration("20230817203308_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("journalapi.Models.Journal", b =>
                {
                    b.Property<Guid>("JournalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ResearcherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JournalId");

                    b.HasIndex("ResearcherId");

                    b.ToTable("Journal", (string)null);
                });

            modelBuilder.Entity("journalapi.Models.Researcher", b =>
                {
                    b.Property<Guid>("ResearcherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResearcherId");

                    b.ToTable("Researcher", (string)null);
                });

            modelBuilder.Entity("journalapi.Models.Subscription", b =>
                {
                    b.Property<Guid>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowedResearcherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ResearcherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("FollowedResearcherId");

                    b.HasIndex("ResearcherId");

                    b.ToTable("Subscription", (string)null);
                });

            modelBuilder.Entity("journalapi.Models.Journal", b =>
                {
                    b.HasOne("journalapi.Models.Researcher", "Researcher")
                        .WithMany("Journals")
                        .HasForeignKey("ResearcherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Researcher");
                });

            modelBuilder.Entity("journalapi.Models.Subscription", b =>
                {
                    b.HasOne("journalapi.Models.Researcher", "FollowedResearcher")
                        .WithMany("Subscriptors")
                        .HasForeignKey("FollowedResearcherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("journalapi.Models.Researcher", "researcher")
                        .WithMany("Subscriptions")
                        .HasForeignKey("ResearcherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FollowedResearcher");

                    b.Navigation("researcher");
                });

            modelBuilder.Entity("journalapi.Models.Researcher", b =>
                {
                    b.Navigation("Journals");

                    b.Navigation("Subscriptions");

                    b.Navigation("Subscriptors");
                });
#pragma warning restore 612, 618
        }
    }
}
