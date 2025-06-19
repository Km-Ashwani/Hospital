using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Auth.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabTechnicianId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_LabTechnicianId",
                table: "Appointments",
                column: "LabTechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_LabTechnicianId",
                table: "Appointments",
                column: "LabTechnicianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_LabTechnicianId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_LabTechnicianId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "LabTechnicianId",
                table: "Appointments");
        }
    }
}
