using Duende.IdentityServer.EntityFramework.Options;
using KSUID;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Crypto.Bot;
using Nummi.Core.Domain.Crypto.Trading.Strategy;
using Nummi.Core.Domain.Crypto.Trading.Strategy.Opportunist;
using Nummi.Core.Domain.Test;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Database;

public class AppDb : ApiAuthorizationDbContext<User> {
    public DbSet<TradingBot> Bots { get; set; } = default!;
    public DbSet<TradingStrategy> Strategies { get; set; } = default!;
    public DbSet<OpportunistStrategy> OpportunistStrategies { get; set; } = default!;
    public DbSet<Blog> Blogs { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;

    public AppDb(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blog>()
            .Property<string?>("PostId");
        
        modelBuilder.Entity<TradingBot>()
            .Property<string?>("StrategyId");
        
        modelBuilder.Entity<TradingBot>()
            .HasOne(b => b.Strategy)
            .WithOne()
            .HasForeignKey<TradingBot>("StrategyId");
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);
        builder.ConvertProperties<Ksuid, KsuidConverter>();
        builder.SerializeToJson<BotError>();
        builder.SerializeToJson<BotErrorHistory>();
        builder.SerializeToJson<OpportunistStrategy.OpportunistParameters>();
        builder.SerializeToJson<OpportunistStrategy.OpportunistState>();
    }
}