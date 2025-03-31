using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiDrivenMedicalPlatformAPIs.Migrations
{
    /// <inheritdoc />
    public partial class addHospitalIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "AspNetUsers");
        }
    }
}
