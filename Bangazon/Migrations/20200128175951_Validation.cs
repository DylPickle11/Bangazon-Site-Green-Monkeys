using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class Validation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2595f2e4-a684-427e-b462-4cd138bdc8fe", "AQAAAAEAACcQAAAAEHIVCwaZP3KZD8xD/yQ3cogLomGeMvHXojX67FG1f5lfydnemDJskNw7dz7XfcLYig==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dddd6950-e099-42c7-a82b-a1e1145be33f", "AQAAAAEAACcQAAAAEKI1HxkkXgt7sHPqTU+ieGBzf4ygfu4RQHYnh1yJvUYDfzEDQC2SeAZwQZpNjZx/NA==" });
        }
    }
}
