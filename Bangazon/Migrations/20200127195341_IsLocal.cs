using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class IsLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocal",
                table: "Product",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dddd6950-e099-42c7-a82b-a1e1145be33f", "AQAAAAEAACcQAAAAEKI1HxkkXgt7sHPqTU+ieGBzf4ygfu4RQHYnh1yJvUYDfzEDQC2SeAZwQZpNjZx/NA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocal",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cacb04b9-cdda-4533-84e5-eb6a062061c0", "AQAAAAEAACcQAAAAEL0BDjLyMwGIEFCiGyYcDSna7hMcVinl2P/W7yu3JEI6kpwJ1NVFEVIWrhT/tilJng==" });
        }
    }
}
