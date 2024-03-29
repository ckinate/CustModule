using BlazorSpinner;
using Fintrak.VendorPortal.Blazor.Client;
using Fintrak.VendorPortal.Blazor.Client.Invitations;
using Fintrak.VendorPortal.Blazor.Client.Onboarding;
using Fintrak.VendorPortal.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Fintrak.VendorPortal.Blazor.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Fintrak.VendorPortal.Blazor.ServerAPI"));

builder.Services.AddApiAuthorization();

builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IOnboardingService, OnboardingService>();

builder.Services.AddScoped<SpinnerService>();

builder.Services.AddSingleton<OnboardingStepService>();

//builder.Services.AddBootstrapBlazor();
builder.Services.AddSyncfusionBlazor();
SyncfusionLicenseProvider.RegisterLicense("OTcwMEAzMjMwMkUzMzJFMzBab0pMcFBROERxYXVFcTRFWWpIWmZBQWtEY3k2NkFiYms4aDhSbnpheTM0PQ==");

await builder.Build().RunAsync();
