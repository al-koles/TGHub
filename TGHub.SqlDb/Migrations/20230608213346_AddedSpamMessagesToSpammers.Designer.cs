﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TGHub.SqlDb;

#nullable disable

namespace TGHub.SqlDb.Migrations
{
    [DbContext(typeof(TgHubDbContext))]
    [Migration("20230608213346_AddedSpamMessagesToSpammers")]
    partial class AddedSpamMessagesToSpammers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TGHub.Domain.Entities.ArchiveBann", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Context")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<int?>("InitiatorId")
                        .HasColumnType("int");

                    b.Property<int>("SpammerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UnBannInitiatorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InitiatorId");

                    b.HasIndex("SpammerId");

                    b.HasIndex("UnBannInitiatorId");

                    b.ToTable("ArchiveBann", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<long?>("LinkedChatTelegramId")
                        .HasColumnType("bigint");

                    b.Property<bool>("ListSpamOn")
                        .HasColumnType("bit");

                    b.Property<string>("LogoFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OffensiveSpamOn")
                        .HasColumnType("bit");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TelegramId")
                        .IsUnique();

                    b.ToTable("Channel", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.ChannelAdministrator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AdministratorId")
                        .HasColumnType("int");

                    b.Property<int>("ChannelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdministratorId");

                    b.HasIndex("ChannelId", "AdministratorId")
                        .IsUnique();

                    b.ToTable("ChannelAdministrator", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Lottery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("AttachmentsFolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AttachmentsFormat")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LotteryTelegramId")
                        .HasColumnType("int");

                    b.Property<int?>("ResultTelegramId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WinnersCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Lottery", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.LotteryAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LotteryId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LotteryId");

                    b.ToTable("LotteryAttachment", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.LotteryParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsWinner")
                        .HasColumnType("bit");

                    b.Property<int>("LotteryId")
                        .HasColumnType("int");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LotteryId", "TelegramId");

                    b.ToTable("LotteryParticipant", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("AttachmentsFolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AttachmentsFormat")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TelegramId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Post", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.PostAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("PostAttachment", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.PostButton", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("PostButton", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Spammer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BannContext")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BannDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("BannInitiatorId")
                        .HasColumnType("int");

                    b.Property<int>("ChannelId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BannInitiatorId");

                    b.HasIndex("ChannelId", "TelegramId")
                        .IsUnique();

                    b.ToTable("Spammer", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.SpamMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTimeWritten")
                        .HasColumnType("datetime2");

                    b.Property<int>("SpammerId")
                        .HasColumnType("int");

                    b.Property<int>("TelegramId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SpammerId", "TelegramId")
                        .IsUnique();

                    b.ToTable("SpamMessage", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.SpamWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ChannelId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("SpamWord", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.TgHubUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TelegramId")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("TGHub.Domain.Entities.ArchiveBann", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.ChannelAdministrator", "Initiator")
                        .WithMany("InitiatedArchiveBanns")
                        .HasForeignKey("InitiatorId");

                    b.HasOne("TGHub.Domain.Entities.Spammer", "Spammer")
                        .WithMany("ArchiveBanns")
                        .HasForeignKey("SpammerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TGHub.Domain.Entities.ChannelAdministrator", "UnBannInitiator")
                        .WithMany("InitiatedArchiveUnBanns")
                        .HasForeignKey("UnBannInitiatorId");

                    b.Navigation("Initiator");

                    b.Navigation("Spammer");

                    b.Navigation("UnBannInitiator");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.ChannelAdministrator", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.TgHubUser", "Administrator")
                        .WithMany("AdministratedChannels")
                        .HasForeignKey("AdministratorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TGHub.Domain.Entities.Channel", "Channel")
                        .WithMany("Administrators")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Administrator");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Lottery", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.ChannelAdministrator", "Creator")
                        .WithMany("Lotteries")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.LotteryAttachment", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.Lottery", "Lottery")
                        .WithMany("Attachments")
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lottery");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.LotteryParticipant", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.Lottery", "Lottery")
                        .WithMany("Participants")
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lottery");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Post", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.ChannelAdministrator", "Creator")
                        .WithMany("Posts")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.PostAttachment", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.Post", "Post")
                        .WithMany("Attachments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.PostButton", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.Post", "Post")
                        .WithMany("Buttons")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Spammer", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.ChannelAdministrator", "BannInitiator")
                        .WithMany("BannedUsers")
                        .HasForeignKey("BannInitiatorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TGHub.Domain.Entities.Channel", "Channel")
                        .WithMany("Spammers")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BannInitiator");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.SpamMessage", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.Spammer", "Spammer")
                        .WithMany("SpamMessages")
                        .HasForeignKey("SpammerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Spammer");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.SpamWord", b =>
                {
                    b.HasOne("TGHub.Domain.Entities.Channel", "Channel")
                        .WithMany("SpamWords")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Channel", b =>
                {
                    b.Navigation("Administrators");

                    b.Navigation("SpamWords");

                    b.Navigation("Spammers");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.ChannelAdministrator", b =>
                {
                    b.Navigation("BannedUsers");

                    b.Navigation("InitiatedArchiveBanns");

                    b.Navigation("InitiatedArchiveUnBanns");

                    b.Navigation("Lotteries");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Lottery", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Post", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Buttons");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.Spammer", b =>
                {
                    b.Navigation("ArchiveBanns");

                    b.Navigation("SpamMessages");
                });

            modelBuilder.Entity("TGHub.Domain.Entities.TgHubUser", b =>
                {
                    b.Navigation("AdministratedChannels");
                });
#pragma warning restore 612, 618
        }
    }
}
