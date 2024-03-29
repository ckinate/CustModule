using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
    public class CustomerProductConfiguration : IEntityTypeConfiguration<CustomerProduct>
	{
		public void Configure(EntityTypeBuilder<CustomerProduct> builder)
		{
			builder.Property(t => t.ProductName).HasMaxLength(200).IsRequired();
			builder.Property(t => t.ProductCode).HasMaxLength(200).IsRequired();
			builder.Property(t => t.Reason).HasMaxLength(500).IsRequired(false);
			builder.Property(t => t.Website).HasMaxLength(300).IsRequired(false);

            builder.Property(t => t.CustomerCode).HasMaxLength(100).IsRequired(false);
            builder.Property(t => t.CustomerMis).HasMaxLength(100).IsRequired(false);
			builder.Property(t => t.Remark).IsRequired(false);

			builder.HasIndex(t => new { t.CustomerId, t.ProductId }).IsUnique();

			builder.ToTable("CustomerProducts");
		}
	}
}
