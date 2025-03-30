using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class C : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "patientList");

            migrationBuilder.AddColumn<double>(
                name: "FinalScore",
                table: "patientFileList",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "patientFileList");

            migrationBuilder.AddColumn<double>(
                name: "FinalScore",
                table: "patientList",
                type: "float",
                nullable: true);
        }
    }
}
