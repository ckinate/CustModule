using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerDocumentConfiguration : IEntityTypeConfiguration<CustomerDocument>
	{
		public void Configure(EntityTypeBuilder<CustomerDocument> builder)
		{
			builder.Property(t => t.CustomerId).IsRequired();
			builder.Property(t => t.Title).HasMaxLength(300).IsRequired();
			builder.Property(t => t.Location).HasMaxLength(500).IsRequired();

			builder.Property(t => t.IssueDate).IsRequired(false);
			builder.Property(t => t.ExpiryDate).IsRequired(false);

			builder.Property(t => t.Remark).HasMaxLength(300).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerId, c.Title });


			builder.ToTable("CustomerDocuments");
		}
	}
}
