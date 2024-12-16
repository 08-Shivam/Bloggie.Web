using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogspot.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5df0f3bc-09d4-4c02-82b2-4c21e6af48bd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a62175b3-cf67-4df6-87a6-8aa18155a7a3", "AQAAAAIAAYagAAAAEIL95rG5WVb7qlNhlYLcCYn6sawPw757FeXRxLsWxxatEj5Yy1JneKyGg+s5aNkS1A==", "5ef1bcb6-e823-4e18-9ff9-b04cb933ef8f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5df0f3bc-09d4-4c02-82b2-4c21e6af48bd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ae0525c5-043f-40a1-8f67-1bb01f55c737", "AQAAAAIAAYagAAAAEOhy0dpiAjDKdBbGSbRQvYkD9XGLPvlNesbvH0GZYSKe6JJ9XI5Pz2/zS9nzvGf8Qw==", "8492babd-eb2c-42da-8f21-2d01aa9fac8c" });
        }
    }
}
