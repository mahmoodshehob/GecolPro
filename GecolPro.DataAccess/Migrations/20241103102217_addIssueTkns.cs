using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GecolPro.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addIssueTkns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IssueTkns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MSISDN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeReq = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeterNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTkns", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueTkns");
        }
    }
}
