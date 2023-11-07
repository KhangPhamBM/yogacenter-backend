using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;

namespace YogaCenter.BackEnd.DAL.Data
{
    public class YogaCenterContext : IdentityDbContext<ApplicationUser>
    {
      
        public YogaCenterContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Attendance> Attendances { get; set; } 
        public DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassDetail> ClassDetails { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<PaymentRespone> PaymentRespones { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionStatus> SubscriptionStatuses { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TimeFrame> TimeFrames { get; set; }
        public DbSet<Room> Rooms { get; set; }
       



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Attendance>()
               .HasKey(pi => new { pi.ClassDetailId, pi.ScheduleId });

            builder.Entity<SubscriptionStatus>().HasData(
                new SubscriptionStatus { SubscriptionStatusId = SD.Subscription.PENDING, SubscriptionStatusName = "Pending" });

            builder.Entity<SubscriptionStatus>().HasData(
              new SubscriptionStatus { SubscriptionStatusId = SD.Subscription.SUCCESSFUL, SubscriptionStatusName = "Successful" });

            builder.Entity<SubscriptionStatus>().HasData(
              new SubscriptionStatus { SubscriptionStatusId = SD.Subscription.FAILED, SubscriptionStatusName = "Failed" });

            builder.Entity<AttendanceStatus>().HasData(
                new AttendanceStatus
                {
                    AttendanceStatusId = 1,
                    AttendanceStatusName = "NOT YET"
                }) ;
            builder.Entity<AttendanceStatus>().HasData(
                new AttendanceStatus
                {
                    AttendanceStatusId = 2,
                    AttendanceStatusName = "Attended"
                });
            builder.Entity<AttendanceStatus>().HasData(
              new AttendanceStatus
              {
                  AttendanceStatusId = 3,
                  AttendanceStatusName = "Absent"
              });

            builder.Entity<TimeFrame>().HasData(
              new TimeFrame { TimeFrameId = SD.TimeFrame.TIMEFRAME_7H, TimeFrameName = "7H00 - 9H00" });
            builder.Entity<TimeFrame>().HasData(
            new TimeFrame { TimeFrameId = SD.TimeFrame.TIMEFRAME_9H, TimeFrameName = "9H00 - 11H00" });
            builder.Entity<TimeFrame>().HasData(
              new TimeFrame { TimeFrameId = SD.TimeFrame.TIMEFRAME_13H, TimeFrameName = "13H00 - 15H00" });
            builder.Entity<TimeFrame>().HasData(
            new TimeFrame { TimeFrameId = SD.TimeFrame.TIMEFRAME_17H, TimeFrameName = "17H00 - 19H00" });
            builder.Entity<TimeFrame>().HasData(
            new TimeFrame { TimeFrameId = SD.TimeFrame.TIMEFRAME_19H, TimeFrameName = "19H00 - 21H00" });

            builder.Entity<PaymentType>().HasData(
            new PaymentType { PaymentTypeId = SD.PaymentType.VNPAY, Type = "VNPAY" });

            builder.Entity<PaymentType>().HasData(
            new PaymentType { PaymentTypeId = SD.PaymentType.MOMO, Type = "MOMO" });

            builder.Entity<Room>().HasData(
           new Room { RoomId = SD.Room.A01, RoomName = "A01" });

            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A02, RoomName = "A02" });

            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A03, RoomName = "A03" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A04, RoomName = "A04" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A05, RoomName = "A05" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A06, RoomName = "A06" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A07, RoomName = "A07" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A08, RoomName = "A08" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A09, RoomName = "A09" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.A10, RoomName = "A10" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B01, RoomName = "B01" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B02, RoomName = "B02" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B03, RoomName = "B03" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B04, RoomName = "B04" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B05, RoomName = "B05" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B06, RoomName = "B06" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B07, RoomName = "B07" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B08, RoomName = "B08" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B09, RoomName = "B09" });
            builder.Entity<Room>().HasData(
          new Room { RoomId = SD.Room.B10, RoomName = "B10" });


            builder.Entity<TicketType>().HasData(
         new TicketType { TicketTypeId = SD.TicketType.REFUND_TICKET, TicketName = "Refund ticket" });

            builder.Entity<TicketType>().HasData(
        new TicketType { TicketTypeId = SD.TicketType.OTHER_TICKET, TicketName = "Other ticket" });


            builder.Entity<TicketStatus>().HasData(
        new TicketStatus { TicketStatusId = SD.TicketStatus.PENDING, TicketStatusName = "Pending" });

            builder.Entity<TicketStatus>().HasData(
        new TicketStatus { TicketStatusId = SD.TicketStatus.APPROVED, TicketStatusName = "Approved" });

            builder.Entity<TicketStatus>().HasData(
        new TicketStatus { TicketStatusId = SD.TicketStatus.REJECTED, TicketStatusName = "Rejected" });

        }
    }
}
