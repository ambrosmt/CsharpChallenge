using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Alpha_Sharp.Data.Migrations
{
    public partial class updateEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeLeft",
                table: "ApplicationAuction",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeLeft",
                table: "ApplicationAuction",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
