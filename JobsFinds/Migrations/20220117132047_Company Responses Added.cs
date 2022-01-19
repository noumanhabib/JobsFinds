using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsFinds.Migrations
{
    public partial class CompanyResponsesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobApplicationId = table.Column<int>(type: "int", nullable: false),
                    InterviewTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewLocation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyResponses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyResponses");
        }
    }
}
