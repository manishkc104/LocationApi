using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MeetUpWebApi.Models
{
    public class MeetUpWebApiContext : DbContext
    {
        public MeetUpWebApiContext (DbContextOptions<MeetUpWebApiContext> options)
            :   base(options)
        {
        }

        //Fluent API 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserLocation>().HasKey(table => new {
                table.UserID,
                table.LocationID
            });

            builder.Entity<UserEvent>().HasKey(table => new {
                table.UserID,
                table.EventID
            });

            builder.Entity<UserEmergency>().HasKey(table => new {
                table.UserID,
                table.EmergencyID
            });

            builder.Entity<UserRelation>().HasKey(table => new {
                table.UserA,
                table.UserB
            });
        }

        public DbSet<MeetUpWebApi.Models.Users> Users { get; set; }
        public DbSet<MeetUpWebApi.Models.Events> Events { get; set; }
        public DbSet<MeetUpWebApi.Models.Locations> Locations { get; set; }
        public DbSet<MeetUpWebApi.Models.Emergency> Emergency { get; set; }
        public DbSet<MeetUpWebApi.Models.UserEvent> UserEvent { get; set; }
        public DbSet<MeetUpWebApi.Models.UserLocation> UserLocation { get; set; }
        public DbSet<MeetUpWebApi.Models.UserEmergency> UserEmergency { get; set; }
        public DbSet<MeetUpWebApi.Models.UserRelation> UserRelation { get; set; }
    }
}
