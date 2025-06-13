using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class modify_AiAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalysisDate",
                table: "AiAnalyses");

            migrationBuilder.DropColumn(
                name: "HeatmapData",
                table: "AiAnalyses");

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "AiAnalyses",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "AiAnalyses");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalysisDate",
                table: "AiAnalyses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "HeatmapData",
                table: "AiAnalyses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
