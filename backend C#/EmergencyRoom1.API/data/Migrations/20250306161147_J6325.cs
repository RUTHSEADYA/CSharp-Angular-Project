using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class J6325 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientList_Treatment_TreatmentId",
                table: "patientList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Treatment",
                table: "Treatment");

            migrationBuilder.RenameTable(
                name: "Treatment",
                newName: "Treatments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Treatments",
                table: "Treatments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_patientList_Treatments_TreatmentId",
                table: "patientList",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientList_Treatments_TreatmentId",
                table: "patientList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Treatments",
                table: "Treatments");

            migrationBuilder.RenameTable(
                name: "Treatments",
                newName: "Treatment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Treatment",
                table: "Treatment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_patientList_Treatment_TreatmentId",
                table: "patientList",
                column: "TreatmentId",
                principalTable: "Treatment",
                principalColumn: "Id");
        }
    }
}
