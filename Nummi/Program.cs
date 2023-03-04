using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nummi.Api.Filters;
using Nummi.Core.App;
using Nummi.Core.App.Bots;
using Nummi.Core.App.Client;
using Nummi.Core.App.Commands;
using Nummi.Core.App.Queries;
using Nummi.Core.App.Simulations;
using Nummi.Core.App.Strategies;
using Nummi.Core.Bridge;
using Nummi.Core.Bridge.DotNet;
using Nummi.Core.Config;
using Nummi.Core.Database.Common;
using Nummi.Core.Database.EFCore;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Test;
using Nummi.Core.Domain.User;
using Nummi.Core.Events;
using Nummi.Core.External.Alpaca;
using Nummi.Core.External.Binance;
using Nummi.Core.External.Coinbase;

void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<EFCoreContext>(options =>
        options.UseNpgsql("Host=localhost;Port=5432;Database=nummi;Username=brandon;Password=password;Include Error Detail=true"));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

void ConfigureIdentities(WebApplicationBuilder builder) {
    builder.Services.AddDefaultIdentity<NummiUser>(options =>
            options.SignIn.RequireConfirmedAccount = false
        )
        .AddRoles<NummiRole>()
        .AddEntityFrameworkStores<EFCoreContext>();

    builder.Services.AddIdentityServer()
        .AddApiAuthorization<NummiUser, EFCoreContext>();

    builder.Services
        .AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
        })
        .AddJwtBearer(o => { 
            o.TokenValidationParameters = new TokenValidationParameters {
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true 
            };
        });
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

// Singletons
builder.Services.AddSingleton<INummiServiceProvider, AspDotNetServiceProvider>();
builder.Services.AddSingleton<StrategyTemplateFactory>();
builder.Services.AddSingleton<EventDispatcher>();

// Services
builder.Services.AddScoped<CryptoDataClientLive>();
builder.Services.AddScoped<CryptoDataClientDbProxy>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<TradingSessionFactory>();
builder.Services.AddScoped<INummiUserManager, AspDotNetUserManager>();

// Commands
builder.Services.AddScoped<ActivateBotCommand>();
builder.Services.AddScoped<ChangeBotStrategyCommand>();
builder.Services.AddScoped<CreateBotCommand>();
builder.Services.AddScoped<DeactivateBotCommand>();
builder.Services.AddScoped<SimulateStrategyCommand>();
builder.Services.AddScoped<ReInitializeBuiltinStrategiesCommand>();
builder.Services.AddScoped<InstantiateStrategyCommand>();

// Queries
builder.Services.AddScoped<GetUserQuery>();
builder.Services.AddScoped<GetStrategyTemplatesQuery>();
builder.Services.AddScoped<GetSimulationsQuery>();
builder.Services.AddScoped<GetOneSimulationQuery>();

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
builder.Services.AddHostedService<NummiInitializer>();
builder.Services.AddHostedService<BotExecutor>();

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
    options.MapType<Ksuid>(() => new OpenApiSchema { Type = "string" });

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