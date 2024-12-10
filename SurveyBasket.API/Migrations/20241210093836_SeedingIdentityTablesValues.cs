using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingIdentityTablesValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0C5D4CA5-B451-4859-9053-44F4D1EDBA26", "8D8A4F7C-FA0E-40D5-AA0B-F272E3B25DC7", false, false, "Admin", "ADMIN" },
                    { "BCED2DD3-C6AE-4586-9575-8B6487F33518", "71BCC04F-9ADB-49A2-A884-ED85FF045B9F", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "88737173-EC5A-4FBF-A25E-4AAE3B812E30", 0, "5B1A6463-39DB-4E7B-8D83-83204CAC9DB4", "admin@survy-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVY-BASKET.COM", "ADMIN@SURVY-BASKET.COM", "AQAAAAIAAYagAAAAEKtP/x6jszKjNsGQ7/FFlkcoP23eOYrIrXDX7WaKF9eehZUW2ocrfEgO4KG0h7D0gA==", null, false, "5A0286EEF32C470D9293147F99C6DF76", false, "admin@survy-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 2, "permissions", "polls:add", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 3, "permissions", "polls:update", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 4, "permissions", "polls:delete", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 5, "permissions", "questions:read", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 6, "permissions", "questions:add", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 7, "permissions", "questions:update", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 8, "permissions", "users:read", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 9, "permissions", "users:add", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 10, "permissions", "users:update", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 11, "permissions", "roles:read", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 12, "permissions", "roles:add", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 13, "permissions", "roles:update", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" },
                    { 14, "permissions", "results:read", "0C5D4CA5-B451-4859-9053-44F4D1EDBA26" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0C5D4CA5-B451-4859-9053-44F4D1EDBA26", "88737173-EC5A-4FBF-A25E-4AAE3B812E30" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "BCED2DD3-C6AE-4586-9575-8B6487F33518");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0C5D4CA5-B451-4859-9053-44F4D1EDBA26", "88737173-EC5A-4FBF-A25E-4AAE3B812E30" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0C5D4CA5-B451-4859-9053-44F4D1EDBA26");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "88737173-EC5A-4FBF-A25E-4AAE3B812E30");
        }
    }
}
