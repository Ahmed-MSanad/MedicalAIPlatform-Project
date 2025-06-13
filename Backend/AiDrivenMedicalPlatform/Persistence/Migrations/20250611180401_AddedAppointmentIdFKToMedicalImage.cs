using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAppointmentIdFKToMedicalImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "MedicalImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalImages_AppointmentId",
                table: "MedicalImages",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalImages_Appointments_AppointmentId",
                table: "MedicalImages",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalImages_Appointments_AppointmentId",
                table: "MedicalImages");

            migrationBuilder.DropIndex(
                name: "IX_MedicalImages_AppointmentId",
                table: "MedicalImages");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "MedicalImages");
        }
    }
}
