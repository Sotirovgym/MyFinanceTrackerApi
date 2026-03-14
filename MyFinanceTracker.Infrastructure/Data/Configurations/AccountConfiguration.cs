using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinanceTracker.Core.Entities;
using MyFinanceTracker.Infrastructure.Identity;

namespace MyFinanceTracker.Infrastructure.Data.Configurations
{
    public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts", "public");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            builder.Property(e => e.AccountType).HasColumnName("account_type").HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Currency).HasColumnName("currency").HasConversion<string>().HasMaxLength(3);
            builder.Property(e => e.Balance).HasColumnName("balance").HasPrecision(18, 2);
            builder.Property(e => e.IsActive).HasColumnName("is_active");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => new { e.UserId, e.IsActive });

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
