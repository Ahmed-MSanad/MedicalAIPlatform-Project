using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifyMedicalImageToHaveManyAnalysisForEachIllness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AiAnalyses_MedicalImageId",
                table: "AiAnalyses");

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "AiAnalyses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_MedicalImageId_Diagnosis",
                table: "AiAnalyses",
                columns: new[] { "MedicalImageId", "Diagnosis" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AiAnalyses_MedicalImageId_Diagnosis",
                table: "AiAnalyses");

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "AiAnalyses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_MedicalImageId",
                table: "AiAnalyses",
                column: "MedicalImageId",
                unique: true);
        }
    }
}