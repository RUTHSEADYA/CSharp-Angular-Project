using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientAttributeList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientAttributeList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "patientList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalScore = table.Column<double>(type: "float", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usersList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "patientFileList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientFileList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patientFileList_patientList_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patientList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientAttributePatientFile",
                columns: table => new
                {
                    PatientAttributesId = table.Column<int>(type: "int", nullable: false),
                    PatientFilesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAttributePatientFile", x => new { x.PatientAttributesId, x.PatientFilesId });
                    table.ForeignKey(
                        name: "FK_ClientAttributePatientFile_clientAttributeList_PatientAttributesId",
                        column: x => x.PatientAttributesId,
                        principalTable: "clientAttributeList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAttributePatientFile_patientFileList_PatientFilesId",
                        column: x => x.PatientFilesId,
                        principalTable: "patientFileList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientAttributePatientFile_PatientFilesId",
                table: "ClientAttributePatientFile",
                column: "PatientFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_patientFileList_PatientId",
                table: "patientFileList",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientAttributePatientFile");

            migrationBuilder.DropTable(
                name: "usersList");

            migrationBuilder.DropTable(
                name: "clientAttributeList");

            migrationBuilder.DropTable(
                name: "patientFileList");

            migrationBuilder.DropTable(
                name: "patientList");
        }
    }
}
