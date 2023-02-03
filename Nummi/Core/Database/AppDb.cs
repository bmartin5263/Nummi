﻿using Duende.IdentityServer.EntityFramework.Options;
using KSUID;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Crypto.Strategies.Log;
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
    public DbSet<Bar> HistoricalBars { get; set; } = default!;
    public DbSet<BotThreadEntity> BotThreads { get; set; } = default!;
    public DbSet<Simulation> Simulations { get; set; } = default!;
    public DbSet<OrderLog> OrderLogs { get; set; } = default!;
    
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
        modelBuilder.OneToOne<Simulation, Strategy>("StrategyId", s => s.Strategy);
        
        modelBuilder.ManyToOne<StrategyLog, Strategy>("StrategyId", l => l.Strategy, s => s.Logs);
        modelBuilder.OneToMany<Bot, Simulation>("BotId", b => b.Simulations);
        modelBuilder.OneToMany<StrategyLog, OrderLog>("StrategyLogId", b => b.Orders);

        modelBuilder.RegisterJsonProperty<Post, Metadata>(p => p.Meta);
        modelBuilder.RegisterJsonProperty<OpportunistStrategy, ISet<string>?>(p => p.Symbols);
        modelBuilder.RegisterJsonProperty<Simulation, List<StrategyLog>>(p => p.Logs);
        modelBuilder.RegisterJsonProperty<OrderLog, CryptoOrderQuantity>(p => p.Quantity);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);
        builder.ConvertProperties<Ksuid, KsuidConverter>();
        builder.ConvertProperties<TradingMode, EnumToStringConverter<TradingMode>>();
        builder.ConvertProperties<StrategyAction, EnumToStringConverter<StrategyAction>>();
        builder.ConvertProperties<SimulationStatus, EnumToStringConverter<SimulationStatus>>();
        builder.ConvertProperties<OrderSide, EnumToStringConverter<OrderSide>>();
        builder.ConvertProperties<OrderType, EnumToStringConverter<OrderType>>();
        builder.ConvertProperties<TimeInForce, EnumToStringConverter<TimeInForce>>();
    }
}