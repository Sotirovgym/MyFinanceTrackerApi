using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameFinanceTablesToLowercase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_UserId",
                schema: "public",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_UserId",
                schema: "public",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_UserId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                schema: "public",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                schema: "public",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Transaction",
                schema: "public",
                newName: "transactions",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "public",
                newName: "categories",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "public",
                newName: "accounts",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserId_TransactionDate",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_UserId_TransactionDate");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserId",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_TransactionDate",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_TransactionDate");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CategoryId",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_AccountId",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_UserId_Type",
                schema: "public",
                table: "categories",
                newName: "IX_categories_UserId_Type");

            migrationBuilder.RenameIndex(
                name: "IX_Category_UserId",
                schema: "public",
                table: "categories",
                newName: "IX_categories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_UserId_IsActive",
                schema: "public",
                table: "accounts",
                newName: "IX_accounts_UserId_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Account_UserId",
                schema: "public",
                table: "accounts",
                newName: "IX_accounts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                schema: "public",
                table: "transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                schema: "public",
                table: "categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                schema: "public",
                table: "accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_AspNetUsers_UserId",
                schema: "public",
                table: "accounts",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_AspNetUsers_UserId",
                schema: "public",
                table: "categories",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_AspNetUsers_UserId",
                schema: "public",
                table: "transactions",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_AccountId",
                schema: "public",
                table: "transactions",
                column: "AccountId",
                principalSchema: "public",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_CategoryId",
                schema: "public",
                table: "transactions",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_AspNetUsers_UserId",
                schema: "public",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_AspNetUsers_UserId",
                schema: "public",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_AspNetUsers_UserId",
                schema: "public",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_AccountId",
                schema: "public",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_CategoryId",
                schema: "public",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                schema: "public",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                schema: "public",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                schema: "public",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "transactions",
                schema: "public",
                newName: "Transaction",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "categories",
                schema: "public",
                newName: "Category",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "accounts",
                schema: "public",
                newName: "Account",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_UserId_TransactionDate",
                schema: "public",
                table: "Transaction",
                newName: "IX_Transaction_UserId_TransactionDate");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_UserId",
                schema: "public",
                table: "Transaction",
                newName: "IX_Transaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_TransactionDate",
                schema: "public",
                table: "Transaction",
                newName: "IX_Transaction_TransactionDate");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_CategoryId",
                schema: "public",
                table: "Transaction",
                newName: "IX_Transaction_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_AccountId",
                schema: "public",
                table: "Transaction",
                newName: "IX_Transaction_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_categories_UserId_Type",
                schema: "public",
                table: "Category",
                newName: "IX_Category_UserId_Type");

            migrationBuilder.RenameIndex(
                name: "IX_categories_UserId",
                schema: "public",
                table: "Category",
                newName: "IX_Category_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_UserId_IsActive",
                schema: "public",
                table: "Account",
                newName: "IX_Account_UserId_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_UserId",
                schema: "public",
                table: "Account",
                newName: "IX_Account_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                schema: "public",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                schema: "public",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                schema: "public",
                table: "Account",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_UserId",
                schema: "public",
                table: "Account",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_UserId",
                schema: "public",
                table: "Category",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                schema: "public",
                table: "Transaction",
                column: "AccountId",
                principalSchema: "public",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_UserId",
                schema: "public",
                table: "Transaction",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                schema: "public",
                table: "Transaction",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
