using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class I6325 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "patientList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "patientList",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Treatment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Procedure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patientList_TreatmentId",
                table: "patientList",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_patientList_Treatment_TreatmentId",
                table: "patientList",
                column: "TreatmentId",
                principalTable: "Treatment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientList_Treatment_TreatmentId",
                table: "patientList");

            migrationBuilder.DropTable(
                name: "Treatment");

            migrationBuilder.DropIndex(
                name: "IX_patientList_TreatmentId",
                table: "patientList");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "patientList");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "patientList");
        }
    }
}
