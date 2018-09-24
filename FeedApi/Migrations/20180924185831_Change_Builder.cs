using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FeedApi.Migrations
{
    public partial class Change_Builder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Feeds",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FeedCollectionsFeeds",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FeedCollections",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FeedCollectionsFeeds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Feeds",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FeedCollections",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "SYSUTCDATETIME()");
        }
    }
}
