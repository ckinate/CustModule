using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class QueryConfiguration : IEntityTypeConfiguration<Query>
	{
		public void Configure(EntityTypeBuilder<Query> builder)
		{
			builder.Property(t => t.ResourceType).IsRequired();
			builder.Property(t => t.ResourceReference).HasMaxLength(200).IsRequired();
			builder.Property(t => t.QueryMessage).HasMaxLength(500).IsRequired();
			builder.Property(t => t.QueryInitiator).HasMaxLength(200).IsRequired();
			builder.Property(t => t.QueryInitiator).HasMaxLength(200).IsRequired();
			builder.Property(t => t.QueryResponse).HasMaxLength(500);

			builder.ToTable("Queries");
		}
	}
}
