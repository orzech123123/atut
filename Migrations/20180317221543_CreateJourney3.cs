using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Atut.Migrations
{
    public partial class CreateJourney3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtherCountriesTotalDistance",
                table: "Journeys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDistance",
                table: "Journeys",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherCountriesTotalDistance",
                table: "Journeys");

            migrationBuilder.DropColumn(
                name: "TotalDistance",
                table: "Journeys");
        }
    }
}
