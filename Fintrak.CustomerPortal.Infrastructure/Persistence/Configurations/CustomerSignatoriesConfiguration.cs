using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerSignatoriesConfiguration : IEntityTypeConfiguration<CustomerSignatory>
	{
		public void Configure(EntityTypeBuilder<CustomerSignatory> builder)
		{
			builder.Property(t => t.CustomerId).IsRequired();
			builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
			builder.Property(t => t.Email).HasMaxLength(300).IsRequired();
			builder.Property(t => t.MobileNumberCallCode).HasMaxLength(10).IsRequired();
			builder.Property(t => t.MobileNumber).HasMaxLength(50).IsRequired();
			builder.Property(t => t.Designation).HasMaxLength(50).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerId, c.Name });

			builder.ToTable("CustomerSignatories");
		}
	}
}
