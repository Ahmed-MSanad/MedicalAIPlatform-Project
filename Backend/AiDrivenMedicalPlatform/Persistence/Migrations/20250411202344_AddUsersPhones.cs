using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersPhones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminPhones",
                columns: table => new
                {
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPhones", x => new { x.Phone, x.AdminId });
                    table.ForeignKey(
                        name: "FK_AdminPhones_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorPhones",
                columns: table => new
                {
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorPhones", x => new { x.Phone, x.DoctorId });
                    table.ForeignKey(
                        name: "FK_DoctorPhones_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPhones",
                columns: table => new
                {
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPhones", x => new { x.Phone, x.PatientId });
                    table.ForeignKey(
                        name: "FK_PatientPhones_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminPhones_AdminId",
                table: "AdminPhones",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPhones_DoctorId",
                table: "DoctorPhones",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPhones_PatientId",
                table: "PatientPhones",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminPhones");

            migrationBuilder.DropTable(
                name: "DoctorPhones");

            migrationBuilder.DropTable(
                name: "PatientPhones");
        }
    }
}
