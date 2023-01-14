using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nummi.Core.Database;
using Nummi.Core.Domain.Crypto.Bot;
using Nummi.Core.Domain.Crypto.Bot.Execution;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.Domain.Crypto.Trading.Strategy;
using Nummi.Core.Domain.User;
using Nummi.Core.External.Alpaca;
using Nummi.Core.External.Coinbase;
using Nummi.Core.External.Cryptowatch;

const string CONNECTION_STRING = "DefaultConnection";   // see appsettings.json

void ConfigureDatabase(WebApplicationBuilder builder)
{
    var dbConnectionString = builder.Configuration.GetConnectionString(CONNECTION_STRING) 
                             ?? throw new InvalidOperationException($"Connection string '{CONNECTION_STRING}' not found.");
    
    builder.Services.AddDbContext<AppDb>(options => options.UseSqlite(dbConnectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

void ConfigureIdentities(WebApplicationBuilder builder)
{
    builder.Services.AddDefaultIdentity<User>(options => 
            options.SignIn.RequireConfirmedAccount = true  // TODO - huh?
        )
        .AddEntityFrameworkStores<AppDb>();

    builder.Services.AddIdentityServer()
        .AddApiAuthorization<User, AppDb>();

    builder.Services.AddAuthentication()
        .AddIdentityServerJwt();
}

var builder = WebApplication.CreateBuilder(args);
ConfigureDatabase(builder);
ConfigureIdentities(builder);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddRazorPages();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BotService>();
builder.Services.AddScoped<StrategyService>();
builder.Services.AddSingleton<MarketDataService>();
builder.Services.AddSingleton<IStockClient, StockClientAlpaca>();
builder.Services.AddSingleton<IAlpacaClient, AlpacaClientPaper>();
builder.Services.AddSingleton<CoinbaseClient>();
builder.Services.AddSingleton<CryptowatchClient>();
builder.Services.AddSingleton<CryptoClientLive>();
builder.Services.AddSingleton<CryptoClientMock>();
builder.Services.AddSingleton<CryptoClientPaper>();
// builder.Services.AddSingleton<IHostedService, BotExecutor2>(_ => new BotExecutor2(new StockBot("Alpha")));
builder.Services.AddSingleton<BotExecutor>(provider => new BotExecutor(provider, 1));
builder.Services.AddSingleton<IHostedService, BotExecutor>(
    serviceProvider => serviceProvider.GetService<BotExecutor>()!);
// builder.Services.AddSingleton<IHostedService, BotExecutor>(_ => new BotExecutor("BotExecutor2"));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Title",
        Description = "Description",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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