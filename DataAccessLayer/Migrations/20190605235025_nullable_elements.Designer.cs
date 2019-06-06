﻿// <auto-generated />
using System;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(HeatingContext))]
    [Migration("20190605235025_nullable_elements")]
    partial class nullable_elements
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataAccessLayer.Entities.Esp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AvgBatteryDuration");

                    b.Property<int>("AvgInteractions");

                    b.Property<string>("ChargeType");

                    b.Property<int>("InteractionsCounter");

                    b.Property<DateTime>("LastCharge");

                    b.Property<DateTime>("LastInteraction");

                    b.HasKey("Id");

                    b.ToTable("Esps");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("DailySetpoint");

                    b.Property<string>("Name");

                    b.Property<float>("NightlySetpoint");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EspId");

                    b.Property<int?>("RoomId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("EspId");

                    b.HasIndex("RoomId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SensorData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Data");

                    b.Property<int>("SensorId");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("SensorDatas");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Sensor", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Esp", "Esp")
                        .WithMany()
                        .HasForeignKey("EspId");

                    b.HasOne("DataAccessLayer.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SensorData", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Sensor", "Sensor")
                        .WithMany()
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
