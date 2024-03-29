using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerCustomFieldConfiguration : IEntityTypeConfiguration<CustomerCustomField>
	{
		public void Configure(EntityTypeBuilder<CustomerCustomField> builder)
		{
			builder.Property(t => t.CustomField).HasMaxLength(300).IsRequired();
			builder.Property(t => t.Response).HasMaxLength(500).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerId, c.CustomFieldId });


			builder.ToTable("CustomerCustomFields");
		}
	}
}
