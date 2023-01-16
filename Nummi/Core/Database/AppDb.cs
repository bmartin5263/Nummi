using Duende.IdentityServer.EntityFramework.Options;
using KSUID;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Execution;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Crypto.Strategies.Opportunist;
using Nummi.Core.Domain.Test;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Database;

public class AppDb : ApiAuthorizationDbContext<User> {
    public DbSet<Bot> Bots { get; set; } = default!;
    public DbSet<Strategy> Strategies { get; set; } = default!;
    public DbSet<OpportunistStrategy> OpportunistStrategies { get; set; } = default!;
    public DbSet<HistoricalPrice> HistoricalPrices { get; set; } = default!;
    public DbSet<BotThreadEntity> BotThreads { get; set; } = default!;
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
        
        modelBuilder.Entity<Bot>()
            .Property<string?>("StrategyId");
        
        modelBuilder.Entity<Bot>()
            .HasOne(b => b.Strategy)
            .WithOne()
            .HasForeignKey<Bot>("StrategyId");
        
        modelBuilder.Entity<BotThreadEntity>()
            .Property<string?>("BotId");
        
        modelBuilder.Entity<BotThreadEntity>()
            .HasOne(b => b.Bot)
            .WithOne()
            .HasForeignKey<BotThreadEntity>("BotId");
        
        modelBuilder.Entity<Strategy>().ToTable(nameof(Strategy));
        modelBuilder.Entity<OpportunistStrategy>().ToTable(nameof(OpportunistStrategy));
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);
        builder.ConvertProperties<Ksuid, KsuidConverter>();
        builder.SerializeToJson<StrategyError>();
        builder.SerializeToJson<StrategyErrorHistory>();
        builder.SerializeToJson<OpportunistStrategy.OpportunistParameters>();
        builder.SerializeToJson<OpportunistStrategy.OpportunistState>();
    }
}