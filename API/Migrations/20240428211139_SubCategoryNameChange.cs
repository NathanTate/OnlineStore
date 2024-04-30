using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class SubCategoryNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_ProductCategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubCategories_ProductCategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductSubCategories");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_CategoryId",
                table: "ProductSubCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_CategoryId",
                table: "ProductSubCategories",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_CategoryId",
                table: "ProductSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSubCategories_CategoryId",
                table: "ProductSubCategories");

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryId",
                table: "ProductSubCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductCategoryId",
                table: "ProductSubCategories",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategories_ProductCategories_ProductCategoryId",
                table: "ProductSubCategories",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }
    }
}
