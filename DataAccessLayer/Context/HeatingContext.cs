using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Context
{
    public class HeatingContext :DbContext
    {
        public DbSet<CommunicationUnit> CommunicationUnits { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Log> Logs { get; set; }

        public HeatingContext(DbContextOptions<HeatingContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(150000);
        }

    }
    public class ToDoContextFactory : IDesignTimeDbContextFactory<HeatingContext>
    {
        public HeatingContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<HeatingContext>();
            // builder.UseMySql("Server=192.168.43.143;Database=HeatingController;User=heatingcontroluser;Password=1werwerwer;", // replace with your Connection String
            builder.UseMySql("Server=localhost;Database=HeatingController;User=root;Password=1werwerwer;", // replace with your Connection String
            mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(8, 0, 15), ServerType.MySql); // replace with your Server Version and Type
                    }
            );
            return new HeatingContext(builder.Options);
        }
    }
}
