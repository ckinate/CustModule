using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class CustomerContactChannelConfiguration : IEntityTypeConfiguration<CustomerContactChannel>
	{
		public void Configure(EntityTypeBuilder<CustomerContactChannel> builder)
		{
			builder.Property(t => t.CustomerId).IsRequired();
			builder.Property(t => t.ChannelType).IsRequired();
			builder.Property(t => t.Email).HasMaxLength(300).IsRequired(false);
			builder.Property(t => t.MobilePhoneCallCode).HasMaxLength(10).IsRequired(false);
			builder.Property(t => t.MobilePhoneNumber).HasMaxLength(50).IsRequired(false);

			builder.HasIndex(c => new { c.CustomerId, c.Email,c.MobilePhoneCallCode,c.MobilePhoneNumber });

			builder.ToTable("CustomerContactChannels");
		}
	}
}
