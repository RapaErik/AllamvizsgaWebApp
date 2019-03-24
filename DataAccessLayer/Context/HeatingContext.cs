using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Context
{
    public class HeatingContext :DbContext
    {
        public DbSet<Esp> Esps { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }

        public HeatingContext(DbContextOptions<HeatingContext> options)
            : base(options)
        {
        }

    }
    public class ToDoContextFactory : IDesignTimeDbContextFactory<HeatingContext>
    {
        public HeatingContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<HeatingContext>();
            builder.UseSqlServer("Server=localhost;Database=DbName;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new HeatingContext(builder.Options);
        }
    }
}
