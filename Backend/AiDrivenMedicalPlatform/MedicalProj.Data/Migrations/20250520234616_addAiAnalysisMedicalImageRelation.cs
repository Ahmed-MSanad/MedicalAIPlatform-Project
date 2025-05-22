using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class addAiAnalysisMedicalImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalImageId",
                table: "AiAnalyses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AiAnalyses_MedicalImageId",
                table: "AiAnalyses",
                column: "MedicalImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AiAnalyses_MedicalImages_MedicalImageId",
                table: "AiAnalyses",
                column: "MedicalImageId",
                principalTable: "MedicalImages",
                principalColumn: "MedicalImageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiAnalyses_MedicalImages_MedicalImageId",
                table: "AiAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_AiAnalyses_MedicalImageId",
                table: "AiAnalyses");

            migrationBuilder.DropColumn(
                name: "MedicalImageId",
                table: "AiAnalyses");
        }
    }
}
