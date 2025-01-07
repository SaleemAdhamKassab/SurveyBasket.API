using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88737173-EC5A-4FBF-A25E-4AAE3B812E30",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEFNVLf45L/MMZhWHyXXDUfp2tBN/S9puvVZ9fHed2/316iPGflwjw+JeuH9BVHy6Rg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88737173-EC5A-4FBF-A25E-4AAE3B812E30",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHepTaKzGfD3n25PSylgMfAnoopV9Y/pP2p1y0nOgbi3nMLr8Tc84xAsHzzLCLDF3Q==");
        }
    }
}
