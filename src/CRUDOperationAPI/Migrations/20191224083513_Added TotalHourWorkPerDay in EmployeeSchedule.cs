using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUDOperationAPI.Migrations
{
    public partial class AddedTotalHourWorkPerDayinEmployeeSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TotalHourWorkPerday",
                table: "EmployeeSchedule",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalHourWorkPerday",
                table: "EmployeeSchedule");
        }
    }
}
