using ECommerce.UI.ApiClients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Account/Login";
        options.AccessDeniedPath = "/Admin/Account/Login";
        options.Cookie.Name = "AdminAuthCookie";
    });
builder.Services.AddRefitClient<ICategoryApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7107/api"));
builder.Services.AddRefitClient<IProductCategoryApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7107/api"));

builder.Services.AddRefitClient<IProductApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7107/api"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 
    
app.Run();
