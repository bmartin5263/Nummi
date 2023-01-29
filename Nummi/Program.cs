using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nummi.Api.Filters;
using Nummi.Core.Database;
using Nummi.Core.Database.Repositories;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Client;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Test;
using Nummi.Core.Domain.User;
using Nummi.Core.External.Alpaca;
using Nummi.Core.External.Binance;
using Nummi.Core.External.Coinbase;

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
            options.SignIn.RequireConfirmedAccount = true
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
builder.Services.AddSingleton<IBinanceClient, BinanceClient>();
builder.Services.AddSingleton<BinanceClientAdapter>();
builder.Services.AddScoped<CryptoDataClientLive>();
builder.Services.AddScoped<CryptoDataClientDbProxy>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<IBarRepository, BarRepository>();
// builder.Services.AddSingleton<IHostedService, BotExecutor2>(_ => new BotExecutor2(new StockBot("Alpha")));
builder.Services.AddSingleton<BotExecutionManager>(provider => new BotExecutionManager(provider, 1));
builder.Services.AddSingleton<IHostedService, BotExecutionManager>(
    serviceProvider => serviceProvider.GetService<BotExecutionManager>()!);
// builder.Services.AddSingleton<IHostedService, BotExecutor>(_ => new BotExecutor("BotExecutor2"));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Nummi",
        Description = "Cryptocurrency Trading",
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

app.UseExceptionHandler(new ExceptionHandlerOptions 
{
    ExceptionHandler = new JsonExceptionMiddleware(app.Environment).Invoke
});

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