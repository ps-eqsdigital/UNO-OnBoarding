using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class UnoOnBoardingContext : DbContext
    {
        public UnoOnBoardingContext(DbContextOptions<UnoOnBoardingContext> options) : base(options)
        {
        }
        public DbSet<Sensor>? Sensors { get; set; }
        public DbSet<SensorData>? SensorsData { get; set; }

        public DbSet<User>? Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Name).UseCollation("Latin1_General_CI_AI");
        }
    }
}
