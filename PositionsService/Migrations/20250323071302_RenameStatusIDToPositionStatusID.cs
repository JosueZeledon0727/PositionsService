using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PositionsService.Migrations
{
    /// <inheritdoc />
    public partial class RenameStatusIDToPositionStatusID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_PositionStatuses_StatusID",
                table: "Positions");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "Positions",
                newName: "PositionStatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_StatusID",
                table: "Positions",
                newName: "IX_Positions_PositionStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_PositionStatuses_PositionStatusID",
                table: "Positions",
                column: "PositionStatusID",
                principalTable: "PositionStatuses",
                principalColumn: "PositionStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_PositionStatuses_PositionStatusID",
                table: "Positions");

            migrationBuilder.RenameColumn(
                name: "PositionStatusID",
                table: "Positions",
                newName: "StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_PositionStatusID",
                table: "Positions",
                newName: "IX_Positions_StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_PositionStatuses_StatusID",
                table: "Positions",
                column: "StatusID",
                principalTable: "PositionStatuses",
                principalColumn: "PositionStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
