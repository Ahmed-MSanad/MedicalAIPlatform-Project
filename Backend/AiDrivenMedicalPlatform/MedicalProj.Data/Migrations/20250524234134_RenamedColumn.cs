using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeopleRated",
                table: "Doctors",
                newName: "TotalRating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalRating",
                table: "Doctors",
                newName: "PeopleRated");
        }
    }
}
