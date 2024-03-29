using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerProductDocumentConfiguration : IEntityTypeConfiguration<CustomerProductDocument>
	{
		public void Configure(EntityTypeBuilder<CustomerProductDocument> builder)
		{
			builder.Property(t => t.DocumentTypeName).HasMaxLength(300).IsRequired();
			builder.Property(t => t.LocationUrl).HasMaxLength(500).IsRequired();

			builder.Property(t => t.IssueDate).IsRequired(false);
			builder.Property(t => t.ExpiryDate).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerProductId, c.DocumentTypeName });


			builder.ToTable("CustomerProductDocuments");
		}
	}
}
