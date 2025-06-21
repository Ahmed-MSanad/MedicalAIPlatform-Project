using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class modify_AiAnalysisTableToAcceptMoreThanAnalysisForOneImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AiAnalyses_MedicalImageId",
                table: "AiAnalyses");

            migrationBuilder.AddColumn<string>(
                name: "DiseaseType",
                table: "AiAnalyses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_MedicalImageId_DiseaseType",
                table: "AiAnalyses",
                columns: new[] { "MedicalImageId", "DiseaseType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AiAnalyses_MedicalImageId_DiseaseType",
                table: "AiAnalyses");

            migrationBuilder.DropColumn(
                name: "DiseaseType",
                table: "AiAnalyses");

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_MedicalImageId",
                table: "AiAnalyses",
                column: "MedicalImageId",
                unique: true);
        }
    }
}
