﻿// <auto-generated />
using System;
using Czeum.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Czeum.DAL.Migrations
{
    [DbContext(typeof(CzeumContext))]
    [Migration("20200326175714_AdditionalUserInfo")]
    partial class AdditionalUserInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Czeum.Core.Domain.SerializedBoard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BoardData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MatchId")
                        .IsUnique();

                    b.ToTable("Boards");

                    b.HasDiscriminator<string>("Discriminator").HasValue("SerializedBoard");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.Achivement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Achivements");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Achivement");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.DirectMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FriendshipId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FriendshipId");

                    b.HasIndex("SenderId");

                    b.ToTable("DirectMessages");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.FriendRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Friendship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("User1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("User2Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("User1Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CurrentPlayerIndex")
                        .HasColumnType("int");

                    b.Property<bool>("IsQuickMatch")
                        .HasColumnType("bit");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WinnerId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Data")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReceiverUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverUserId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.StoredMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("SenderId");

                    b.ToTable("MatchMessages");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastDisconnected")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MoveCount")
                        .HasColumnType("int");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.UserAchivement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AchivementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsStarred")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UnlockedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AchivementId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAchivements");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.UserMatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PlayerIndex")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMatches");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Boards.SerializedChessBoard", b =>
                {
                    b.HasBaseType("Czeum.Core.Domain.SerializedBoard");

                    b.HasDiscriminator().HasValue("SerializedChessBoard");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Boards.SerializedConnect4Board", b =>
                {
                    b.HasBaseType("Czeum.Core.Domain.SerializedBoard");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("SerializedConnect4Board");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.DoMovesAchivement", b =>
                {
                    b.HasBaseType("Czeum.Domain.Entities.Achivements.Achivement");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("MoveCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("DoMovesAchivement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11b89499-17be-4c93-b561-b6cf176f1a52"),
                            Level = 1,
                            MoveCount = 1
                        },
                        new
                        {
                            Id = new Guid("b23e061b-fd81-4335-a843-e34bc78679b1"),
                            Level = 2,
                            MoveCount = 500
                        },
                        new
                        {
                            Id = new Guid("5f86c4fe-8d0b-40bf-be7f-e5f374184d9c"),
                            Level = 3,
                            MoveCount = 5000
                        });
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.HaveWinRateAchivement", b =>
                {
                    b.HasBaseType("Czeum.Domain.Entities.Achivements.Achivement");

                    b.Property<int>("Level")
                        .HasColumnName("HaveWinRateAchivement_Level")
                        .HasColumnType("int");

                    b.Property<double>("WinRate")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("HaveWinRateAchivement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("68e19be4-e065-4231-9806-bf40a9d0b004"),
                            Level = 1,
                            WinRate = 0.59999999999999998
                        },
                        new
                        {
                            Id = new Guid("855904dd-16ae-41ee-a5c2-cdfc6f4a914f"),
                            Level = 2,
                            WinRate = 0.69999999999999996
                        },
                        new
                        {
                            Id = new Guid("d62bf078-d3e3-40c5-92cb-a6dda1246327"),
                            Level = 3,
                            WinRate = 0.80000000000000004
                        });
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.WinChessMatchesAchivement", b =>
                {
                    b.HasBaseType("Czeum.Domain.Entities.Achivements.Achivement");

                    b.Property<int>("Level")
                        .HasColumnName("WinChessMatchesAchivement_Level")
                        .HasColumnType("int");

                    b.Property<int>("WinCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("WinChessMatchesAchivement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("205a0c88-45e4-4bf4-bb0a-be66c18dcd86"),
                            Level = 1,
                            WinCount = 1
                        },
                        new
                        {
                            Id = new Guid("dc3fd2c0-c173-4012-b51d-e64fdd7680eb"),
                            Level = 2,
                            WinCount = 25
                        },
                        new
                        {
                            Id = new Guid("e18f8521-0978-4cf4-b123-e88bccaf992e"),
                            Level = 3,
                            WinCount = 100
                        });
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.WinConnect4MatchesAchivement", b =>
                {
                    b.HasBaseType("Czeum.Domain.Entities.Achivements.Achivement");

                    b.Property<int>("Level")
                        .HasColumnName("WinConnect4MatchesAchivement_Level")
                        .HasColumnType("int");

                    b.Property<int>("WinCount")
                        .HasColumnName("WinConnect4MatchesAchivement_WinCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("WinConnect4MatchesAchivement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9982fdd1-8026-4e55-a448-62218cfa4b0a"),
                            Level = 1,
                            WinCount = 1
                        },
                        new
                        {
                            Id = new Guid("22c4ee6b-7451-4c81-8010-0ab28571daf7"),
                            Level = 2,
                            WinCount = 25
                        },
                        new
                        {
                            Id = new Guid("24d59ba8-ff4e-442c-b76f-fbf524e5514d"),
                            Level = 3,
                            WinCount = 100
                        });
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.WinMatchesAchivement", b =>
                {
                    b.HasBaseType("Czeum.Domain.Entities.Achivements.Achivement");

                    b.Property<int>("Level")
                        .HasColumnName("WinMatchesAchivement_Level")
                        .HasColumnType("int");

                    b.Property<int>("WinCount")
                        .HasColumnName("WinMatchesAchivement_WinCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("WinMatchesAchivement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("69d3d5b7-de45-4bda-b20f-0f4cd5ac3340"),
                            Level = 1,
                            WinCount = 10
                        },
                        new
                        {
                            Id = new Guid("d742e2b6-8dd3-4fb8-b80a-728d686251b4"),
                            Level = 2,
                            WinCount = 100
                        },
                        new
                        {
                            Id = new Guid("5bcac04d-2bc4-40ad-af06-4e8cff05945f"),
                            Level = 3,
                            WinCount = 1000
                        });
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Achivements.WinQuickMatchesAchivement", b =>
                {
                    b.HasBaseType("Czeum.Domain.Entities.Achivements.Achivement");

                    b.Property<int>("Level")
                        .HasColumnName("WinQuickMatchesAchivement_Level")
                        .HasColumnType("int");

                    b.Property<int>("WinCount")
                        .HasColumnName("WinQuickMatchesAchivement_WinCount")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("WinQuickMatchesAchivement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("afa121e1-6dd5-49ea-88d2-5cfd538256f5"),
                            Level = 1,
                            WinCount = 5
                        },
                        new
                        {
                            Id = new Guid("7ce1cbff-3fca-4728-9be1-6f632a5e9358"),
                            Level = 2,
                            WinCount = 50
                        },
                        new
                        {
                            Id = new Guid("12944108-9bb0-43e3-b1c8-f3eac1331442"),
                            Level = 3,
                            WinCount = 500
                        });
                });

            modelBuilder.Entity("Czeum.Core.Domain.SerializedBoard", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.Match", null)
                        .WithOne("Board")
                        .HasForeignKey("Czeum.Core.Domain.SerializedBoard", "MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Czeum.Domain.Entities.DirectMessage", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.Friendship", "Friendship")
                        .WithMany("Messages")
                        .HasForeignKey("FriendshipId");

                    b.HasOne("Czeum.Domain.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.FriendRequest", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", "Receiver")
                        .WithMany("ReceivedRequests")
                        .HasForeignKey("ReceiverId");

                    b.HasOne("Czeum.Domain.Entities.User", "Sender")
                        .WithMany("SentRequests")
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Friendship", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", "User1")
                        .WithMany("User1Friendships")
                        .HasForeignKey("User1Id");

                    b.HasOne("Czeum.Domain.Entities.User", "User2")
                        .WithMany("User2Friendships")
                        .HasForeignKey("User2Id");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Match", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", "Winner")
                        .WithMany("WonMatches")
                        .HasForeignKey("WinnerId");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.Notification", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", "ReceiverUser")
                        .WithMany("ReceivedNotifications")
                        .HasForeignKey("ReceiverUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Czeum.Domain.Entities.User", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderUserId");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.StoredMessage", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.Match", "Match")
                        .WithMany("Messages")
                        .HasForeignKey("MatchId");

                    b.HasOne("Czeum.Domain.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Czeum.Domain.Entities.UserAchivement", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.Achivements.Achivement", "Achivement")
                        .WithMany()
                        .HasForeignKey("AchivementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Czeum.Domain.Entities.User", "User")
                        .WithMany("UserAchivements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Czeum.Domain.Entities.UserMatch", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.Match", "Match")
                        .WithMany("Users")
                        .HasForeignKey("MatchId");

                    b.HasOne("Czeum.Domain.Entities.User", "User")
                        .WithMany("Matches")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Czeum.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Czeum.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}