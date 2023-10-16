﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YogaCenter.BackEnd.DAL.Data;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    [DbContext(typeof(YogaCenterContext))]
    partial class YogaCenterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool?>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Attendance", b =>
                {
                    b.Property<int>("ClassDetailId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<bool>("isAttended")
                        .HasColumnType("bit");

                    b.HasKey("ClassDetailId", "ScheduleId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"), 1L, 1);

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MaxOfTrainer")
                        .HasColumnType("int");

                    b.Property<int>("MinOfTrainee")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ClassId");

                    b.HasIndex("CourseId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.ClassDetail", b =>
                {
                    b.Property<int>("ClassDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassDetailId"), 1L, 1);

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ClassDetailId");

                    b.HasIndex("ClassId");

                    b.HasIndex("UserId");

                    b.ToTable("ClassDetails");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"), 1L, 1);

                    b.Property<string>("CourseDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Discount")
                        .HasColumnType("float");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.PaymentRespone", b =>
                {
                    b.Property<string>("PaymentResponseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Amount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriptionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("Success")
                        .HasColumnType("bit");

                    b.HasKey("PaymentResponseId");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("PaymentRespones");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.PaymentType", b =>
                {
                    b.Property<int?>("PaymentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("PaymentTypeId"), 1L, 1);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentTypeId");

                    b.ToTable("PaymentTypes");

                    b.HasData(
                        new
                        {
                            PaymentTypeId = 1,
                            Type = "VNPAY"
                        },
                        new
                        {
                            PaymentTypeId = 2,
                            Type = "MOMO"
                        });
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"), 1L, 1);

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            RoomId = 1,
                            RoomName = "A01"
                        },
                        new
                        {
                            RoomId = 2,
                            RoomName = "A02"
                        },
                        new
                        {
                            RoomId = 3,
                            RoomName = "A03"
                        },
                        new
                        {
                            RoomId = 4,
                            RoomName = "A04"
                        },
                        new
                        {
                            RoomId = 5,
                            RoomName = "A05"
                        },
                        new
                        {
                            RoomId = 6,
                            RoomName = "A06"
                        },
                        new
                        {
                            RoomId = 7,
                            RoomName = "A07"
                        },
                        new
                        {
                            RoomId = 8,
                            RoomName = "A08"
                        },
                        new
                        {
                            RoomId = 9,
                            RoomName = "A09"
                        },
                        new
                        {
                            RoomId = 10,
                            RoomName = "A10"
                        },
                        new
                        {
                            RoomId = 11,
                            RoomName = "B01"
                        },
                        new
                        {
                            RoomId = 12,
                            RoomName = "B02"
                        },
                        new
                        {
                            RoomId = 13,
                            RoomName = "B03"
                        },
                        new
                        {
                            RoomId = 14,
                            RoomName = "B04"
                        },
                        new
                        {
                            RoomId = 15,
                            RoomName = "B05"
                        },
                        new
                        {
                            RoomId = 16,
                            RoomName = "B06"
                        },
                        new
                        {
                            RoomId = 17,
                            RoomName = "B07"
                        },
                        new
                        {
                            RoomId = 18,
                            RoomName = "B08"
                        },
                        new
                        {
                            RoomId = 19,
                            RoomName = "B09"
                        },
                        new
                        {
                            RoomId = 20,
                            RoomName = "B10"
                        });
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleId"), 1L, 1);

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int?>("TimeFrameId")
                        .HasColumnType("int");

                    b.HasKey("ScheduleId");

                    b.HasIndex("ClassId");

                    b.HasIndex("RoomId");

                    b.HasIndex("TimeFrameId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Subscription", b =>
                {
                    b.Property<string>("SubscriptionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ClassId")
                        .HasColumnType("int")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("SubscriptionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubscriptionStatusId")
                        .HasColumnType("int");

                    b.Property<double?>("Total")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("ClassId");

                    b.HasIndex("SubscriptionStatusId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.SubscriptionStatus", b =>
                {
                    b.Property<int>("SubscriptionStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionStatusId"), 1L, 1);

                    b.Property<string>("SubscriptionStatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubscriptionStatusId");

                    b.ToTable("SubscriptionStatuses");

                    b.HasData(
                        new
                        {
                            SubscriptionStatusId = 3,
                            SubscriptionStatusName = "Pending"
                        },
                        new
                        {
                            SubscriptionStatusId = 1,
                            SubscriptionStatusName = "Successful"
                        },
                        new
                        {
                            SubscriptionStatusId = 2,
                            SubscriptionStatusName = "Failed"
                        });
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"), 1L, 1);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TicketStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("TicketTypeId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TicketId");

                    b.HasIndex("TicketStatusId");

                    b.HasIndex("TicketTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.TicketStatus", b =>
                {
                    b.Property<int>("TicketStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketStatusId"), 1L, 1);

                    b.Property<string>("TicketStatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TicketStatusId");

                    b.ToTable("TicketStatuses");

                    b.HasData(
                        new
                        {
                            TicketStatusId = 1,
                            TicketStatusName = "Pending"
                        },
                        new
                        {
                            TicketStatusId = 2,
                            TicketStatusName = "Approved"
                        },
                        new
                        {
                            TicketStatusId = 3,
                            TicketStatusName = "Rejected"
                        });
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.TicketType", b =>
                {
                    b.Property<int>("TicketTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketTypeId"), 1L, 1);

                    b.Property<string>("TicketName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TicketTypeId");

                    b.ToTable("TicketTypes");

                    b.HasData(
                        new
                        {
                            TicketTypeId = 1,
                            TicketName = "Refund ticket"
                        },
                        new
                        {
                            TicketTypeId = 2,
                            TicketName = "Other ticket"
                        });
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.TimeFrame", b =>
                {
                    b.Property<int>("TimeFrameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimeFrameId"), 1L, 1);

                    b.Property<string>("TimeFrameName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TimeFrameId");

                    b.ToTable("TimeFrames");

                    b.HasData(
                        new
                        {
                            TimeFrameId = 1,
                            TimeFrameName = "7H00 - 9H00"
                        },
                        new
                        {
                            TimeFrameId = 2,
                            TimeFrameName = "9H00 - 11H00"
                        },
                        new
                        {
                            TimeFrameId = 3,
                            TimeFrameName = "13H00 - 15H00"
                        },
                        new
                        {
                            TimeFrameId = 4,
                            TimeFrameName = "17H00 - 19H00"
                        },
                        new
                        {
                            TimeFrameId = 5,
                            TimeFrameName = "19H00 - 21H00"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Attendance", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ClassDetail", "ClassDetail")
                        .WithMany()
                        .HasForeignKey("ClassDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassDetail");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Class", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.ClassDetail", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Class");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.PaymentRespone", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId");

                    b.Navigation("PaymentType");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Schedule", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.TimeFrame", "TimeFrame")
                        .WithMany()
                        .HasForeignKey("TimeFrameId");

                    b.Navigation("Class");

                    b.Navigation("Room");

                    b.Navigation("TimeFrame");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Subscription", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.SubscriptionStatus", "SubscriptionStatus")
                        .WithMany()
                        .HasForeignKey("SubscriptionStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("SubscriptionStatus");
                });

            modelBuilder.Entity("YogaCenter.BackEnd.DAL.Models.Ticket", b =>
                {
                    b.HasOne("YogaCenter.BackEnd.DAL.Models.TicketStatus", "TicketStatus")
                        .WithMany()
                        .HasForeignKey("TicketStatusId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.TicketType", "TicketType")
                        .WithMany()
                        .HasForeignKey("TicketTypeId");

                    b.HasOne("YogaCenter.BackEnd.DAL.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("TicketStatus");

                    b.Navigation("TicketType");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
