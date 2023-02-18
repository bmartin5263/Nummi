using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nummi.Api.Filters;
using Nummi.Core.Database.Common;
using Nummi.Core.Database.EFCore;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.Commands;
using Nummi.Core.Domain.New.Data;
using Nummi.Core.Domain.New.Queries;
using Nummi.Core.Domain.Test;
using Nummi.Core.External.Alpaca;
using Nummi.Core.External.Binance;
using Nummi.Core.External.Coinbase;

void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<EFCoreContext>(options =>
        options.UseNpgsql("Host=localhost;Port=5432;Database=nummi;Username=brandon;Password=password"));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

void ConfigureIdentities(WebApplicationBuilder builder)
{
    builder.Services.AddDefaultIdentity<NummiUser>(options => 
            options.SignIn.RequireConfirmedAccount = true
        )
        .AddEntityFrameworkStores<EFCoreContext>();

    builder.Services.AddIdentityServer()
        .AddApiAuthorization<NummiUser, EFCoreContext>();

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

// External Clients
builder.Services.AddSingleton<AlpacaClientLive>();
builder.Services.AddSingleton<AlpacaClientPaper>();
builder.Services.AddSingleton<CoinbaseClient>();
builder.Services.AddSingleton<IBinanceClient, BinanceClient>();
builder.Services.AddSingleton<BinanceClientAdapter>();

// Services
builder.Services.AddScoped<CryptoDataClientLive>();
builder.Services.AddScoped<CryptoDataClientDbProxy>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<TradingContextFactory>();

// Commands
builder.Services.AddScoped<ActivateBotCommand>();
builder.Services.AddScoped<ChangeBotStrategyCommand>();
builder.Services.AddScoped<CreateBotCommand>();
builder.Services.AddScoped<DeactivateBotCommand>();
builder.Services.AddScoped<SimulateStrategyCommand>();

// Queries
builder.Services.AddScoped<GetUserQuery>();

// Repositories + Database
builder.Services.AddScoped<IBarRepository, BarRepository>();
builder.Services.AddScoped<IBotRepository, BotRepository>();
builder.Services.AddScoped<IStrategyRepository, StrategyRepository>();
builder.Services.AddScoped<IStrategyTemplateRepository, StrategyTemplateRepository>();
builder.Services.AddScoped<ISimulationRepository, SimulationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransaction, EFCoreTransaction>();

// Bot Execution Threads
// builder.Services.AddSingleton<BotExecutionManager>(provider => new BotExecutionManager(provider, 1));
// builder.Services.AddSingleton<IHostedService, BotExecutionManager>(
//     serviceProvider => serviceProvider.GetService<BotExecutionManager>()!);

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
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
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
    ExceptionHandler = new JsonExceptionMiddleware().Invoke
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();