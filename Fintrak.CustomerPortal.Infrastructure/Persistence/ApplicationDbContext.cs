using Fintrak.CustomerPortal.Infrastructure.Identity;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MediatR;
using System.Reflection;
using Fintrak.CustomerPortal.Infrastructure.Persistence.Interceptors;
using Fintrak.CustomerPortal.Domain.Entities;
using Fintrak.CustomerPortal.Application.Common.Interfaces;

namespace Fintrak.CustomerPortal.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
	private readonly IMediator _mediator;
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

	//public ApplicationDbContext(
	//       DbContextOptions options,
	//       IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
	//   {
	//   }

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IOptions<OperationalStoreOptions> operationalStoreOptions,
		IMediator mediator,
		AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
		: base(options, operationalStoreOptions)
	{
		_mediator = mediator;
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
	}

	public DbSet<Invitation> Invitations => Set<Invitation>();
	public DbSet<Query> Queries => Set<Query>();

	public DbSet<Customer> Customers => Set<Customer>();
	public DbSet<CustomerAccount> CustomerAccounts => Set<CustomerAccount>();
	public DbSet<CustomerContactChannel> CustomerContactChannels => Set<CustomerContactChannel>();
	public DbSet<CustomerContactPerson> CustomerContactPersons => Set<CustomerContactPerson>();
    public DbSet<CustomerCustomField> CustomerCustomFields => Set<CustomerCustomField>();
    public DbSet<CustomerDocument> CustomerDocuments => Set<CustomerDocument>();
	public DbSet<CustomerSignatory> CustomerSignatories => Set<CustomerSignatory>();

	public DbSet<CustomerProduct> CustomerProducts => Set<CustomerProduct>();
	public DbSet<CustomerProductDocument> CustomerProductDocuments => Set<CustomerProductDocument>();
	public DbSet<CustomerProductCustomField> CustomerProductCustomFields => Set<CustomerProductCustomField>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(builder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _mediator.DispatchDomainEvents(this);

		return await base.SaveChangesAsync(cancellationToken);
	}
}
