using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
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
}
