using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.API.Data.Migrations
{
    public partial class deafultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a403737-064e-461a-bbfa-ef97d4c6b9ac", "a005f181-865f-4bf7-8986-425072269b38", "User", "USER" },
                    { "377b04f4-20fc-462f-a399-c7fcb693b789", "18e17bff-6258-41be-9afa-50d7bcdb3e29", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Pakistan", "PK" },
                    { 2, "Sudia", "SD" },
                    { 3, "India", "IND" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "Lahore", 2, "Stalive", 4.5 },
                    { 2, "New York", 1, "Shangai", 4.5 },
                    { 3, "Iceland", 3, "Polo", 4.5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a403737-064e-461a-bbfa-ef97d4c6b9ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "377b04f4-20fc-462f-a399-c7fcb693b789");

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
