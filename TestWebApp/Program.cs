using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using TestWebApp.Data;
using TestWebApp.Domain;
using TestWebApp.Domain.Model;

const string CONNECTION_STRING = "DefaultConnection";   // see appsettings.json

void ConfigureDatabase(WebApplicationBuilder builder)
{
    var dbConnectionString = builder.Configuration.GetConnectionString(CONNECTION_STRING) 
                             ?? throw new InvalidOperationException($"Connection string '{CONNECTION_STRING}' not found.");
    
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(dbConnectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

void ConfigureIdentities(WebApplicationBuilder builder)
{
    builder.Services.AddDefaultIdentity<User>(options => 
            options.SignIn.RequireConfirmedAccount = true  // TODO - huh?
        )
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddIdentityServer()
        .AddApiAuthorization<User, ApplicationDbContext>();

    builder.Services.AddAuthentication()
        .AddIdentityServerJwt();
}

var builder = WebApplication.CreateBuilder(args);
ConfigureDatabase(builder);
ConfigureIdentities(builder);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<TradeService>();
builder.Services.AddSingleton<AlpacaClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();