using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Atut.Migrations
{
    public partial class VehicleRegistrationNumberAndUserUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RegistrationNumber_UserId",
                table: "Vehicles",
                columns: new[] { "RegistrationNumber", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_RegistrationNumber_UserId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
