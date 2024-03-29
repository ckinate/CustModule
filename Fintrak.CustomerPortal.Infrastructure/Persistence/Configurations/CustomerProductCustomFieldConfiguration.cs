using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerProductCustomFieldConfiguration : IEntityTypeConfiguration<CustomerProductCustomField>
	{
		public void Configure(EntityTypeBuilder<CustomerProductCustomField> builder)
		{
			builder.Property(t => t.CustomField).HasMaxLength(300).IsRequired();
			builder.Property(t => t.Response).HasMaxLength(500).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerProductId, c.CustomFieldId });


			builder.ToTable("CustomerProductCustomFields");
		}
	}
}
