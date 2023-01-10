using Duende.IdentityServer.EntityFramework.Options;
using KSUID;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Crypto.Bot;
using Nummi.Core.Domain.Crypto.Bot.Strategy;
using Nummi.Core.Domain.User;

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