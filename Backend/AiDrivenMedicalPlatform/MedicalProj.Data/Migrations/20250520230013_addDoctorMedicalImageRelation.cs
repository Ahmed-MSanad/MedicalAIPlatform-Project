using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class addDoctorMedicalImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Did",
                table: "MedicalImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalImages_Did",
                table: "MedicalImages",
                column: "Did");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalImages_Doctors_Did",
                table: "MedicalImages",
                column: "Did",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalImages_Doctors_Did",
                table: "MedicalImages");

            migrationBuilder.DropIndex(
                name: "IX_MedicalImages_Did",
                table: "MedicalImages");

            migrationBuilder.DropColumn(
                name: "Did",
                table: "MedicalImages");
        }
    }
}
