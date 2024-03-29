using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
	{
		public void Configure(EntityTypeBuilder<CustomerAccount> builder)
		{
			builder.Property(t => t.CustomerId).IsRequired();
			builder.Property(t => t.BankName).HasMaxLength(200).IsRequired();
			builder.Property(t => t.BankCode).HasMaxLength(50).IsRequired();
			builder.Property(t => t.BankAddress).HasMaxLength(500).IsRequired(false);
			builder.Property(t => t.AccountName).HasMaxLength(200).IsRequired();
			builder.Property(t => t.AccountNumber).HasMaxLength(200).IsRequired();
			builder.Property(t => t.Country).HasMaxLength(200).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerId, c.AccountNumber, c.AccountType });

			builder.ToTable("CustomerAccounts");
		}
	}
}
