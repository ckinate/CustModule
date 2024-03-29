using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Infrastructure.Identity;
using Fintrak.CustomerPortal.Infrastructure.Persistence.Interceptors;
using Fintrak.CustomerPortal.Infrastructure.Persistence;
using Fintrak.CustomerPortal.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Fintrak.CustomerPortal.Infrastructure.Files;

namespace Fintrak.CustomerPortal.Infrastructure
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var portalUrl = configuration["PortalUrl"];

			services.AddScoped<AuditableEntitySaveChangesInterceptor>();

			if (configuration.GetValue<bool>("UseInMemoryDatabase"))
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseInMemoryDatabase("FintrakCustomerPortaldb"));
			}
			else
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
						builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
			}

			services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

			services.AddScoped<ApplicationDbContextInitialiser>();

			services
				.AddDefaultIdentity<ApplicationUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = true;
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = true;
					options.Password.RequireNonAlphanumeric = true;
					options.Password.RequireUppercase = true;
					options.Password.RequiredLength = 10;
					//options.Password.RequiredUniqueChars = 5;
				})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddIdentityServer(options =>
			{
                options.IssuerUri = portalUrl;
			})
		    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

			services.AddAuthentication()
				//.AddJwtBearer(options =>
				//{
				//	options.Authority = portalUrl;
				//})
				 .AddIdentityServerJwt();

			services.AddTransient<IDateTime, DateTimeService>();
			services.AddTransient<IIdentityService, IdentityService>();
			services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
			services.AddTransient<IFileStore, FileStore>();
			services.AddTransient<IEmailService, EmailService>();

			services.AddAuthorization(options =>
				options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

			return services;
		}
	}
}
