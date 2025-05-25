using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class addPatientMedicalImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pid",
                table: "MedicalImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalImages_Pid",
                table: "MedicalImages",
                column: "Pid");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalImages_Patients_Pid",
                table: "MedicalImages",
                column: "Pid",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalImages_Patients_Pid",
                table: "MedicalImages");

            migrationBuilder.DropIndex(
                name: "IX_MedicalImages_Pid",
                table: "MedicalImages");

            migrationBuilder.DropColumn(
                name: "Pid",
                table: "MedicalImages");
        }
    }
}
