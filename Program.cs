using BarberHub.Client;
using BarberHub.Client.Core.Api;
using BarberHub.Client.Core.Auth;
using BarberHub.Client.Core.UI;
using BarberHub.Client.Features.Account.Services;
using BarberHub.Client.Features.Barbers.Services;
using BarberHub.Client.Features.Catalog.Services;
using BarberHub.Client.Features.Gallery.Services;
using BarberHub.Client.Features.PlatformAdmin.Services;
using BarberHub.Client.Features.Salons.Services;
using BarberHub.Client.Features.WorkSchedules.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// آدرس API از wwwroot/appsettings.json خوانده می‌شود؛ مسیرها زیر api/{version}/ هستند.
var apiOptions = new ApiOptions();
builder.Configuration.GetSection("Api").Bind(apiOptions);
builder.Services.AddSingleton(apiOptions);

// --- Auth (Singleton: چون DelegatingHandlerها در scope جداگانه ساخته می‌شوند) ---
builder.Services.AddSingleton<TokenStore>();
builder.Services.AddSingleton<TokenRefresher>();
builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<BarberHubAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BarberHubAuthStateProvider>());

// کلاینت بدون هندلر احراز هویت: فقط برای login/register/refresh/revoke
builder.Services.AddHttpClient(ApiOptions.AnonymousClient, c => c.BaseAddress = apiOptions.ApiRoot);

// کلاینت اصلی: هدر Bearer را اضافه می‌کند و در صورت نیاز توکن را تمدید می‌کند
builder.Services.AddHttpClient<ApiClient>(c => c.BaseAddress = apiOptions.ApiRoot)
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ToastService>();

// --- Feature services ---
builder.Services.AddScoped<UserApi>();
builder.Services.AddScoped<EndUserApi>();
builder.Services.AddScoped<OtpApi>();
builder.Services.AddScoped<SalonApi>();
builder.Services.AddScoped<BarberApi>();
builder.Services.AddScoped<BarberServiceApi>();
builder.Services.AddScoped<ServiceCatalogApi>();
builder.Services.AddScoped<WorkScheduleApi>();
builder.Services.AddScoped<GalleryApi>();
builder.Services.AddScoped<FileApi>();
builder.Services.AddScoped<SalonAdminApi>();
builder.Services.AddScoped<PlatformAdminApi>();
builder.Services.AddScoped<ExceptionLogApi>();

await builder.Build().RunAsync();
