using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsFinds.Migrations
{
    public partial class JobAppupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "JobApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "JobApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "JobApplications");
        }
    }
}
