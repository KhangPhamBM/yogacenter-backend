using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Data
{
    public class YogaCenterContext : IdentityDbContext<ApplicationUser>
    {
        public YogaCenterContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassDetail> ClassDetails { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<PaymentRespone> PaymentRespones { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Subcription> Subcriptions { get; set; }
        public DbSet<SubcriptionStatus> SubcriptionStatuses { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TimeFrame> TimeFrames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Attendance>()
               .HasKey(pi => new { pi.ClassDetailId, pi.ScheduleId });
        }
    }
}
