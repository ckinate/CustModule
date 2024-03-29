using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.Property(t => t.Code).HasMaxLength(200).IsRequired(false);
			builder.Property(t => t.CustomCode).HasMaxLength(200).IsRequired(false);
			builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
			builder.Property(t => t.RegistrationCertificateNumber).HasMaxLength(50).IsRequired();
			builder.Property(t => t.IncorporationDate).IsRequired();
			builder.Property(t => t.RegisterAddress1).HasMaxLength(500).IsRequired();
			builder.Property(t => t.Country).HasMaxLength(200).IsRequired();
			builder.Property(t => t.State).HasMaxLength(200).IsRequired();
			builder.Property(t => t.City).HasMaxLength(200).IsRequired();
			builder.Property(t => t.BusinessNature).HasMaxLength(300).IsRequired();

			builder.Property(t => t.OfficePhoneCallCode).HasMaxLength(10).IsRequired();
			builder.Property(t => t.OfficePhoneNumber).HasMaxLength(50).IsRequired();

			builder.Property(t => t.MobilePhoneCallCode).HasMaxLength(10).IsRequired();
			builder.Property(t => t.MobilePhoneNumber).HasMaxLength(50).IsRequired();

			builder.Property(t => t.Email).HasMaxLength(300).IsRequired();
			builder.Property(t => t.Fax).HasMaxLength(200).IsRequired();
			builder.Property(t => t.Website).HasMaxLength(300).IsRequired();

			builder.Property(t => t.SectorId).IsRequired();
			builder.Property(t => t.SectorName).HasMaxLength(200).IsRequired();

			builder.Property(t => t.IndustryId).IsRequired();
			builder.Property(t => t.IndustryName).HasMaxLength(200).IsRequired();

			builder.Property(t => t.InstitutionTypeId).IsRequired();
			builder.Property(t => t.InstitutionTypeName).HasMaxLength(200).IsRequired();

			builder.Property(t => t.ChildInstitutionTypeName).HasMaxLength(200).IsRequired(false);
			builder.Property(t => t.InstitutionCode).HasMaxLength(200).IsRequired(false);

			builder.Property(t => t.LoginId).HasMaxLength(200).IsRequired();

			builder.Property(t => t.SettlementBankCode).HasMaxLength(50).IsRequired(false);
			builder.Property(t => t.SettlementBankName).HasMaxLength(100).IsRequired(false);

			builder.Property(t => t.Fax).HasMaxLength(200).IsRequired(false);
            builder.Property(t => t.Website).HasMaxLength(200).IsRequired(false);
            builder.Property(t => t.TaxIdentificationNumber).HasMaxLength(200).IsRequired(false);

			builder.Property(t => t.LogoLocation).HasMaxLength(300).IsRequired(false);

			builder.HasMany(c => c.CustomerProducts).WithOne(c => c.Customer).OnDelete(DeleteBehavior.Restrict);
			builder.HasMany(c => c.CustomerContactPersons).WithOne(c => c.Customer).OnDelete( DeleteBehavior.Restrict);

			builder.Property(t => t.ParentCode).HasMaxLength(200).IsRequired(false);
			builder.Property(t => t.ParentName).HasMaxLength(200).IsRequired(false);


			//builder.HasIndex(t => t.TaxIdentificationNumber).IsUnique();
			//builder.HasIndex(t => t.RegistrationCertificateNumber).IsUnique();
			//builder.HasIndex(t => t.Email).IsUnique();
			//builder.HasIndex(t => new { t.OfficePhoneCallCode, t.OfficePhoneNumber }).IsUnique();
			//builder.HasIndex(t => new { t.MobilePhoneCallCode, t.MobilePhoneNumber }).IsUnique();

			builder.ToTable("Customers");
		}
	}
}
