using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GecolPro.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateMetersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccNo",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNo",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaysLastPurchase",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocRef",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Meters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccNo",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "ContactNo",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "DaysLastPurchase",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "LocRef",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Meters");
        }
    }
}
