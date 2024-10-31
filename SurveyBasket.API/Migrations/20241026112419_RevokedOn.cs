using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Migrations
{
    /// <inheritdoc />
    public partial class RevokedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefokedOn",
                table: "RefreshTokens",
                newName: "RevokedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevokedOn",
                table: "RefreshTokens",
                newName: "RefokedOn");
        }
    }
}
