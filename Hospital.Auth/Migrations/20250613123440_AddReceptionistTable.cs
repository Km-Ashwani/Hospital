using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Auth.Migrations
{
    /// <inheritdoc />
    public partial class AddReceptionistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceptionistId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ReceptionistId",
                table: "Appointments",
                column: "ReceptionistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_ReceptionistId",
                table: "Appointments",
                column: "ReceptionistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_ReceptionistId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ReceptionistId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ReceptionistId",
                table: "Appointments");
        }
    }
}
