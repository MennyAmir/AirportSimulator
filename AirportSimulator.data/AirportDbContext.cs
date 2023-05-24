using AirportSimulator.data.DTO;
using AirportSimulator.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AirportSimulator.data
{
    public class AirportDbContext : DbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> options) : base(options) { }


        public DbSet<AirplaneDbDto> Airplanes { get; set; }
        public DbSet<StationDbDto> Stations { get; set; }//real time
        public DbSet<Visit> Visits { get; set; }//history
        public DbSet<AirplaneDbDto> PlannedLandings { get; set; }
        public DbSet<AirplaneDbDto> PlannedTakeOff { get; set; }
        public DbSet<StationDbDto> StationsStatus { get; set; }//real time


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<StationDbDto>().HasData(
                    new { id = 1, Name = "before landing" },
                    new { id = 2, Name = "Preparing for landing" },
                    new { id = 3, Name = "landing" },
                    new { id = 4, Name = "on track" },
                    new { id = 5, Name = "landed" },
                    new { id = 6, Name = "in the terminal" },
                    new { id = 7, Name = "in the terminal" },
                    new { id = 8, Name = "before the appearance" },
                    new { id = 9, Name = "teekoff" }
                );
        }


    }
}
