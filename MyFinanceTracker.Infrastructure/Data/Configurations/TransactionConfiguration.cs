using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinanceTracker.Core.Entities;
using MyFinanceTracker.Infrastructure.Identity;

namespace MyFinanceTracker.Infrastructure.Data.Configurations
{
    public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions", "public");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(e => e.AccountId).HasColumnName("account_id");
            builder.Property(e => e.CategoryId).HasColumnName("category_id");
            builder.Property(e => e.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(16);
            builder.Property(e => e.Amount).HasColumnName("amount").HasPrecision(18, 2);
            builder.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.AccountId);
            builder.HasIndex(e => e.CategoryId);
            builder.HasIndex(e => e.TransactionDate);
            builder.HasIndex(e => new { e.UserId, e.TransactionDate });

            builder.HasOne(e => e.Account)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
