using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace EFarming.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Farm",
                columns: table => new
                {
                    FarmId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farm", x => x.FarmId);
                });

            migrationBuilder.CreateTable(
                name: "PredefinedValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MinValue = table.Column<double>(nullable: false),
                    MaxValue = table.Column<double>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    ValueType = table.Column<string>(nullable: true),
                    ValueFor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActuatorId = table.Column<int>(nullable: false),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FarmZone",
                columns: table => new
                {
                    ZoneId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    FarmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmZone", x => x.ZoneId);
                    table.ForeignKey(
                        name: "FK_FarmZone_Farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farm",
                        principalColumn: "FarmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Actuator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    CriticalValue = table.Column<double>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    ValveOpenTime = table.Column<DateTime>(nullable: false),
                    WaterFlowRate = table.Column<double>(nullable: false),
                    IsGoodCondition = table.Column<bool>(nullable: false),
                    FarmZoneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actuator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actuator_FarmZone_FarmZoneId",
                        column: x => x.FarmZoneId,
                        principalTable: "FarmZone",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IrrigationMode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Mode = table.Column<int>(nullable: false),
                    FarmZoneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IrrigationMode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IrrigationMode_FarmZone_FarmZoneId",
                        column: x => x.FarmZoneId,
                        principalTable: "FarmZone",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    CriticalValue = table.Column<double>(nullable: false),
                    ActuatorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensor_Actuator_ActuatorId",
                        column: x => x.ActuatorId,
                        principalTable: "Actuator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActuatorId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: true),
                    IrrigationModeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedData_Actuator_ActuatorId",
                        column: x => x.ActuatorId,
                        principalTable: "Actuator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessedData_IrrigationMode_IrrigationModeId",
                        column: x => x.IrrigationModeId,
                        principalTable: "IrrigationMode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessedData_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Signal = table.Column<double>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    SensorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorData_Sensor_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actuator_FarmZoneId",
                table: "Actuator",
                column: "FarmZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmZone_FarmId",
                table: "FarmZone",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_IrrigationMode_FarmZoneId",
                table: "IrrigationMode",
                column: "FarmZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedData_ActuatorId",
                table: "ProcessedData",
                column: "ActuatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedData_IrrigationModeId",
                table: "ProcessedData",
                column: "IrrigationModeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedData_StateId",
                table: "ProcessedData",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_ActuatorId",
                table: "Sensor",
                column: "ActuatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_SensorId",
                table: "SensorData",
                column: "SensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredefinedValues");

            migrationBuilder.DropTable(
                name: "ProcessedData");

            migrationBuilder.DropTable(
                name: "SensorData");

            migrationBuilder.DropTable(
                name: "IrrigationMode");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "Actuator");

            migrationBuilder.DropTable(
                name: "FarmZone");

            migrationBuilder.DropTable(
                name: "Farm");
        }
    }
}
