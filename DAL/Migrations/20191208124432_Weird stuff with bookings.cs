using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Weirdstuffwithbookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isFinished",
                table: "Bookings",
                newName: "IsFinished");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBeginning",
                table: "Bookings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfReturn",
                table: "Bookings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBeginning",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DateOfReturn",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "IsFinished",
                table: "Bookings",
                newName: "isFinished");
        }
    }
}
