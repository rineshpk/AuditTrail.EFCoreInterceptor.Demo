using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Auditing.Migrations
{
    public partial class CorrelationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "SaveChangesAudit",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Succeeded = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveChangesAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditHistory",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Changed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SaveChangesAuditId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditHistory_SaveChangesAudit_SaveChangesAuditId",
                        column: x => x.SaveChangesAuditId,
                        principalSchema: "Audit",
                        principalTable: "SaveChangesAudit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditHistory_SaveChangesAuditId",
                schema: "Audit",
                table: "AuditHistory",
                column: "SaveChangesAuditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditHistory",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "SaveChangesAudit",
                schema: "Audit");
        }
    }
}
