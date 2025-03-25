using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PositionsService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DepartmentName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "PositionStatuses",
                columns: table => new
                {
                    PositionStatusID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionStatuses", x => x.PositionStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Recruiters",
                columns: table => new
                {
                    RecruiterID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecruiterName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruiters", x => x.RecruiterID);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    StatusID = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentID = table.Column<int>(type: "INTEGER", nullable: false),
                    RecruiterID = table.Column<int>(type: "INTEGER", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionID);
                    table.CheckConstraint("CK_Position_Budget", "[Budget] >= 0");
                    table.ForeignKey(
                        name: "FK_Positions_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_PositionStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "PositionStatuses",
                        principalColumn: "PositionStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_Recruiters_RecruiterID",
                        column: x => x.RecruiterID,
                        principalTable: "Recruiters",
                        principalColumn: "RecruiterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentID", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "HR" },
                    { 2, "IT" },
                    { 3, "Sales" }
                });

            migrationBuilder.InsertData(
                table: "PositionStatuses",
                columns: new[] { "PositionStatusID", "StatusName" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Inactive" },
                    { 3, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "Recruiters",
                columns: new[] { "RecruiterID", "RecruiterName" },
                values: new object[,]
                {
                    { 1, "Pancho Villa" },
                    { 2, "Juan Ramirez" },
                    { 3, "Federico Alvarez" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Positions_DepartmentID",
                table: "Positions",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_PositionNumber",
                table: "Positions",
                column: "PositionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_RecruiterID",
                table: "Positions",
                column: "RecruiterID");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_StatusID",
                table: "Positions",
                column: "StatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "PositionStatuses");

            migrationBuilder.DropTable(
                name: "Recruiters");
        }
    }
}
