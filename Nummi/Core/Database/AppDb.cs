using Duende.IdentityServer.EntityFramework.Options;
using KSUID;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Crypto.Strategies.Opportunist;
using Nummi.Core.Domain.Test;
using Nummi.Core.Domain.User;

namespace Nummi.Core.Database;

public class AppDb : ApiAuthorizationDbContext<User> {
    public DbSet<Bot> Bots { get; set; } = default!;
    public DbSet<Strategy> Strategies { get; set; } = default!;
    public DbSet<StrategyLog> StrategyLogs { get; set; } = default!;
    public DbSet<OpportunistStrategy> OpportunistStrategies { get; set; } = default!;
    public DbSet<Price> HistoricalPrices { get; set; } = default!;
    public DbSet<MinuteBar> HistoricalMinuteBars { get; set; } = default!;
    public DbSet<BotThreadEntity> BotThreads { get; set; } = default!;
    public DbSet<Blog> Blogs { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;

    public AppDb(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // TBT Strategy
        modelBuilder.Entity<Strategy>().ToTable(nameof(Strategy));
        modelBuilder.Entity<OpportunistStrategy>().ToTable(nameof(OpportunistStrategy));

        modelBuilder.OneToOne<Blog, Post>("PostId", b => b.Post);
        modelBuilder.OneToOne<Bot, Strategy>("StrategyId", b => b.Strategy);
        modelBuilder.OneToOne<Bot, StrategyLog>("LastStrategyLogId", b => b.LastStrategyLog);
        modelBuilder.OneToOne<BotThreadEntity, Bot>("BotId", b => b.Bot);
        modelBuilder.OneToMany<StrategyLog, Strategy>("StrategyId", l => l.Strategy, s => s.Logs);
        modelBuilder.RegisterJsonProperty<Post, Metadata>(p => p.Meta);
        modelBuilder.RegisterJsonProperty<OpportunistStrategy, ISet<string>?>(p => p.Symbols);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);
        builder.ConvertProperties<Ksuid, KsuidConverter>();
        builder.ConvertProperties<TradingEnvironment, EnumToStringConverter<TradingEnvironment>>();
    }
}