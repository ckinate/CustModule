using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Server.Filters;
using Fintrak.CustomerPortal.Blazor.Server.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;

namespace Fintrak.CustomerPortal.Blazor.Server
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddSingleton<ICurrentUserService, CurrentUserService>();

			var allowOrigins = configuration["AllowOrigins"].Split(",");

            services.AddCors(opt =>
			{
				opt.AddPolicy(name: "mySpecificOrigins",  policy =>
                {
					policy.WithOrigins(allowOrigins)
					.AllowAnyHeader()
					.AllowAnyMethod();
					//.AllowCredentials();
				});
			});

			services.AddHttpContextAccessor();

			//services.AddHealthChecks()
			//	.AddDbContextCheck<ApplicationDbContext>();

			//services.AddControllersWithViews(options =>
			//	options.Filters.Add<ApiExceptionFilterAttribute>())
			//		.AddFluentValidation(x => x.AutomaticValidationEnabled = false);

			services.AddControllersWithViews(options =>
				options.Filters.Add<ApiExceptionFilterAttribute>())
				.AddJsonOptions(options =>
				{
					//options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				});

			services.AddRazorPages();

			services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

			//services.Configure<ForwardedHeadersOptions>(options =>
			//{
			//	options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			//});

			// Customise default API behaviour
			services.Configure<ApiBehaviorOptions>(options =>
				options.SuppressModelStateInvalidFilter = true);

			services.AddSwaggerGen(setupAction =>
			{
				setupAction.SwaggerDoc(
					$"CustomerPortal",
					new OpenApiInfo()
					{
						Title = "Customer Portal API",
						Version = "1",
						Description = "Customer Portal API Specification.",
						Contact = new OpenApiContact
						{
							Email = "abc.xyz@gmail.com",
							Name = "Developer",
							Url = new Uri("https://github.com/customerportal"),
						},
						License = new OpenApiLicense
						{
							Name = "License",
							Url = new Uri("https://customerportal.com/licenses"),
						},
					});

				setupAction.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
				{
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Description = "Input your Bearer token to access this API",
				});

				setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "bearer",
							},
						}, new List<string>()
					},
				});
			});

			services.AddScoped<IRegisterService, RegisterService>();

			return services;
		}
	}
}
