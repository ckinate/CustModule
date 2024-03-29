using Fintrak.CustomerPortal.Application;
using Fintrak.CustomerPortal.Infrastructure;
using Fintrak.CustomerPortal.Blazor.Server;
using Swashbuckle.AspNetCore.SwaggerUI;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using Fintrak.CustomerPortal.Infrastructure.Integration;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"customer_log.txt")
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\ProjectDigital_webhost_api_log.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

//var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIntegrationServices(builder.Configuration);
builder.Services.AddWebUIServices(builder.Configuration);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentityServer()
//    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

//builder.Services.AddAuthentication()
//    .AddIdentityServerJwt();

// VERY IMPORTANT FOR THE DOCUSIGN WEBHOOKS
builder.Services.Configure<KestrelServerOptions>(options =>
{
	options.AllowSynchronousIO = true;
});

builder.Services.Configure<IISServerOptions>(options =>
{
	options.AllowSynchronousIO = true;
});

var app = builder.Build();

var portalUrl = builder.Configuration["PortalUrl"];

_ = app.Use(async (ctx, next) =>
            {
                ctx.SetIdentityServerOrigin(portalUrl);
                await next();
            });


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
	//app.UseForwardedHeaders();
}
else
{
    app.UseExceptionHandler("/Error");
	//app.UseForwardedHeaders();
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseSwagger();

app.UseSwaggerUI(setupAction =>
{
	setupAction.SwaggerEndpoint("/swagger/CustomerPortal/swagger.json", "Customer Portal API v1");

	setupAction.DefaultModelExpandDepth(2);
	setupAction.DefaultModelRendering(ModelRendering.Model);
	setupAction.DocExpansion(DocExpansion.None);
	setupAction.EnableDeepLinking();
	setupAction.DisplayOperationId();
});

app.UseRouting();
app.UseCors("mySpecificOrigins");

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
//app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
