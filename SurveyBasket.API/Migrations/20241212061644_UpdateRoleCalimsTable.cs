using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleCalimsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88737173-EC5A-4FBF-A25E-4AAE3B812E30",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHepTaKzGfD3n25PSylgMfAnoopV9Y/pP2p1y0nOgbi3nMLr8Tc84xAsHzzLCLDF3Q==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88737173-EC5A-4FBF-A25E-4AAE3B812E30",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKtP/x6jszKjNsGQ7/FFlkcoP23eOYrIrXDX7WaKF9eehZUW2ocrfEgO4KG0h7D0gA==");
        }
    }
}
