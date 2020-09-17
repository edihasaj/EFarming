﻿// <auto-generated />
using System;
using EFarming.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFarming.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200526151333_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EFarming.Models.Actuator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("CriticalValue")
                        .HasColumnType("double");

                    b.Property<int>("FarmZoneId")
                        .HasColumnType("int");

                    b.Property<bool>("IsGoodCondition")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<DateTime>("ValveOpenTime")
                        .HasColumnType("datetime");

                    b.Property<double>("WaterFlowRate")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("FarmZoneId");

                    b.ToTable("Actuator");
                });

            modelBuilder.Entity("EFarming.Models.Farm", b =>
                {
                    b.Property<int>("FarmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("FarmId");

                    b.ToTable("Farm");
                });

            modelBuilder.Entity("EFarming.Models.FarmZone", b =>
                {
                    b.Property<int>("ZoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int>("FarmId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ZoneId");

                    b.HasIndex("FarmId");

                    b.ToTable("FarmZone");
                });

            modelBuilder.Entity("EFarming.Models.IrrigationMode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FarmZoneId")
                        .HasColumnType("int");

                    b.Property<int>("Mode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmZoneId");

                    b.ToTable("IrrigationMode");
                });

            modelBuilder.Entity("EFarming.Models.PredefinedValues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("MaxValue")
                        .HasColumnType("double");

                    b.Property<double>("MinValue")
                        .HasColumnType("double");

                    b.Property<double>("Value")
                        .HasColumnType("double");

                    b.Property<int>("ValueFor")
                        .HasColumnType("int");

                    b.Property<string>("ValueType")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PredefinedValues");
                });

            modelBuilder.Entity("EFarming.Models.ProcessedData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActuatorId")
                        .HasColumnType("int");

                    b.Property<int?>("IrrigationModeId")
                        .HasColumnType("int");

                    b.Property<int?>("StateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActuatorId");

                    b.HasIndex("IrrigationModeId");

                    b.HasIndex("StateId");

                    b.ToTable("ProcessedData");
                });

            modelBuilder.Entity("EFarming.Models.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ActuatorId")
                        .HasColumnType("int");

                    b.Property<double>("CriticalValue")
                        .HasColumnType("double");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActuatorId");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("EFarming.Models.SensorData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("SensorId")
                        .HasColumnType("int");

                    b.Property<double>("Signal")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("SensorData");
                });

            modelBuilder.Entity("EFarming.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActuatorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("State");
                });

            modelBuilder.Entity("EFarming.Models.Actuator", b =>
                {
                    b.HasOne("EFarming.Models.FarmZone", "FarmZone")
                        .WithMany("Actuators")
                        .HasForeignKey("FarmZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFarming.Models.FarmZone", b =>
                {
                    b.HasOne("EFarming.Models.Farm", "Farm")
                        .WithMany("Zones")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFarming.Models.IrrigationMode", b =>
                {
                    b.HasOne("EFarming.Models.FarmZone", "FarmZone")
                        .WithMany()
                        .HasForeignKey("FarmZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFarming.Models.ProcessedData", b =>
                {
                    b.HasOne("EFarming.Models.Actuator", "Actuator")
                        .WithMany("ProcessedData")
                        .HasForeignKey("ActuatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFarming.Models.IrrigationMode", "IrrigationMode")
                        .WithMany()
                        .HasForeignKey("IrrigationModeId");

                    b.HasOne("EFarming.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId");
                });

            modelBuilder.Entity("EFarming.Models.Sensor", b =>
                {
                    b.HasOne("EFarming.Models.Actuator", null)
                        .WithMany("Sensors")
                        .HasForeignKey("ActuatorId");
                });

            modelBuilder.Entity("EFarming.Models.SensorData", b =>
                {
                    b.HasOne("EFarming.Models.Sensor", "Sensor")
                        .WithMany("Data")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}