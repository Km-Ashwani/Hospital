using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Auth.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceptionistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReceptionistDetailsReceptionistId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "receptionistDetails",
                columns: table => new
                {
                    ReceptionistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYear = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receptionistDetails", x => x.ReceptionistId);
                    table.ForeignKey(
                        name: "FK_receptionistDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ReceptionistDetailsReceptionistId",
                table: "Appointments",
                column: "ReceptionistDetailsReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_receptionistDetails_UserId",
                table: "receptionistDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_receptionistDetails_ReceptionistDetailsReceptionistId",
                table: "Appointments",
                column: "ReceptionistDetailsReceptionistId",
                principalTable: "receptionistDetails",
                principalColumn: "ReceptionistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_receptionistDetails_ReceptionistDetailsReceptionistId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "receptionistDetails");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ReceptionistDetailsReceptionistId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ReceptionistDetailsReceptionistId",
                table: "Appointments");
        }
    }
}
