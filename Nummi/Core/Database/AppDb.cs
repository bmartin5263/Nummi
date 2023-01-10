using Alpaca.Markets;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.Models;
using KSUID;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nummi.Core.Domain.Stocks.Ordering;
using Nummi.Core.Domain.User;
using Nummi.Core.Domain.Stocks;
using Nummi.Core.Domain.Stocks.Bot;
using Nummi.Core.Domain.Stocks.Bot.Strategy;

namespace Nummi.Core.Database;

public class AppDb : ApiAuthorizationDbContext<User> {
    public DbSet<TradingBot> Bots { get; set; } = default!;
    
    public AppDb(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
        base.ConfigureConventions(configurationBuilder);
        
        configurationBuilder
            .Properties<ITradingStrategy>()
            .HaveConversion<TradingStrategyConverter>();        
        
        configurationBuilder
            .Properties<BotError>()
            .HaveConversion<GenericJsonConverter<BotError>>();        
        
        configurationBuilder
            .Properties<Ksuid>()
            .HaveConversion<KsuidConverter>();
    }
}