using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekVerse.Server.Migrations
{
    public partial class ProductVisibleDeleteFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ProductVariant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "ProductVariant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 1, 2 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 1, 3 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 1, 4 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 2, 2 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 3, 2 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 4, 2 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 5, 1 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 5, 2 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 5, 3 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 6, 5 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 7, 8 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 7, 9 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 7, 10 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 8, 6 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 8, 7 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 9, 6 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 9, 7 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 10, 8 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 11, 8 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 12, 9 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 13, 10 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariant",
                keyColumns: new[] { "ProductId", "ProductTypeId" },
                keyValues: new object[] { 14, 1 },
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "Visible",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Products");
        }
    }
}
