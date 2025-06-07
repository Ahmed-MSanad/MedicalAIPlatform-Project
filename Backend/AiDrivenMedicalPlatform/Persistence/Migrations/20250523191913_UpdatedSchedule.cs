using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules");

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "DoctorSchedules",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules",
                columns: new[] { "DoctorId", "Day" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedules",
                column: "DoctorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedules");

            migrationBuilder.AlterColumn<int>(
                name: "Day",
                table: "DoctorSchedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules",
                column: "DoctorId");
        }
    }
}
