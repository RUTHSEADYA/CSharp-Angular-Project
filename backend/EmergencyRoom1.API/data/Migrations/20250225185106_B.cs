using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class B : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "patientFileList",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Descreption",
                table: "patientFileList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descreption",
                table: "patientFileList");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "patientFileList",
                newName: "status");
        }
    }
}
