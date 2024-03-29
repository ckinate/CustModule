using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
	public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
	{
		public void Configure(EntityTypeBuilder<Invitation> builder)
		{
			builder.Property(t => t.Code).HasMaxLength(200).IsRequired();
			builder.Property(t => t.CompanyName).HasMaxLength(200).IsRequired();
			builder.Property(t => t.AdminName).HasMaxLength(200).IsRequired();
			builder.Property(t => t.AdminEmail).HasMaxLength(300).IsRequired();

			builder.ToTable("Invitations");
		}
	}
}
