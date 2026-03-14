using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameFinanceColumnsToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "public",
                table: "transactions",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "public",
                table: "transactions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "public",
                table: "transactions",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "transactions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "transactions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                schema: "public",
                table: "transactions",
                newName: "transaction_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "transactions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                schema: "public",
                table: "transactions",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "public",
                table: "transactions",
                newName: "account_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_UserId_TransactionDate",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_user_id_transaction_date");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_UserId",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_TransactionDate",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_transaction_date");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_CategoryId",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_category_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_AccountId",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_account_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "public",
                table: "categories",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "categories",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "categories",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "public",
                table: "categories",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "categories",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_categories_UserId_Type",
                schema: "public",
                table: "categories",
                newName: "IX_categories_user_id_type");

            migrationBuilder.RenameIndex(
                name: "IX_categories_UserId",
                schema: "public",
                table: "categories",
                newName: "IX_categories_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "accounts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "public",
                table: "accounts",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Balance",
                schema: "public",
                table: "accounts",
                newName: "balance");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "accounts",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "accounts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "public",
                table: "accounts",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "accounts",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AccountType",
                schema: "public",
                table: "accounts",
                newName: "account_type");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_UserId_IsActive",
                schema: "public",
                table: "accounts",
                newName: "IX_accounts_user_id_is_active");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_UserId",
                schema: "public",
                table: "accounts",
                newName: "IX_accounts_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_AspNetUsers_user_id",
                schema: "public",
                table: "accounts",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_AspNetUsers_user_id",
                schema: "public",
                table: "categories",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_AspNetUsers_user_id",
                schema: "public",
                table: "transactions",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_account_id",
                schema: "public",
                table: "transactions",
                column: "account_id",
                principalSchema: "public",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_category_id",
                schema: "public",
                table: "transactions",
                column: "category_id",
                principalSchema: "public",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_AspNetUsers_user_id",
                schema: "public",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_AspNetUsers_user_id",
                schema: "public",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_AspNetUsers_user_id",
                schema: "public",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_account_id",
                schema: "public",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_category_id",
                schema: "public",
                table: "transactions");

            migrationBuilder.RenameColumn(
                name: "type",
                schema: "public",
                table: "transactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "description",
                schema: "public",
                table: "transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "amount",
                schema: "public",
                table: "transactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "public",
                table: "transactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "public",
                table: "transactions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                schema: "public",
                table: "transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "public",
                table: "transactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_id",
                schema: "public",
                table: "transactions",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "account_id",
                schema: "public",
                table: "transactions",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_user_id_transaction_date",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_UserId_TransactionDate");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_user_id",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_transaction_date",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_TransactionDate");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_category_id",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_account_id",
                schema: "public",
                table: "transactions",
                newName: "IX_transactions_AccountId");

            migrationBuilder.RenameColumn(
                name: "type",
                schema: "public",
                table: "categories",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "public",
                table: "categories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "public",
                table: "categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                schema: "public",
                table: "categories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "public",
                table: "categories",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_categories_user_id_type",
                schema: "public",
                table: "categories",
                newName: "IX_categories_UserId_Type");

            migrationBuilder.RenameIndex(
                name: "IX_categories_user_id",
                schema: "public",
                table: "categories",
                newName: "IX_categories_UserId");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "accounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "currency",
                schema: "public",
                table: "accounts",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "balance",
                schema: "public",
                table: "accounts",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "public",
                table: "accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "public",
                table: "accounts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "public",
                table: "accounts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                schema: "public",
                table: "accounts",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "public",
                table: "accounts",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "account_type",
                schema: "public",
                table: "accounts",
                newName: "AccountType");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_user_id_is_active",
                schema: "public",
                table: "accounts",
                newName: "IX_accounts_UserId_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_user_id",
                schema: "public",
                table: "accounts",
                newName: "IX_accounts_UserId");

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
    }
}
