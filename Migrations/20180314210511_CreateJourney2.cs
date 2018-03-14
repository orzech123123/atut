using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Atut.Migrations
{
    public partial class CreateJourney2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountOfPeople",
                table: "Journeys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Journeys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FinalPlace",
                table: "Journeys",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Journeys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ThroughPlace",
                table: "Journeys",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfPeople",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "FinalPlace",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "ThroughPlace",
                table: "Journeys");
        }
    }
}
