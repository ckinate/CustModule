using Fintrak.CustomerPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Invitation> Invitations { get; }
    DbSet<Query> Queries { get; }

	DbSet<Customer> Customers { get; }
	DbSet<CustomerAccount> CustomerAccounts { get; }
	DbSet<CustomerContactChannel> CustomerContactChannels { get; }
	DbSet<CustomerContactPerson> CustomerContactPersons { get; }
    DbSet<CustomerCustomField> CustomerCustomFields { get; }
    DbSet<CustomerDocument> CustomerDocuments { get; }
	DbSet<CustomerSignatory> CustomerSignatories { get; }

	DbSet<CustomerProduct> CustomerProducts { get; }
	DbSet<CustomerProductDocument> CustomerProductDocuments { get; }
	DbSet<CustomerProductCustomField> CustomerProductCustomFields { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
