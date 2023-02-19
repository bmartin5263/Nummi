using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.Test;

namespace Nummi.Core.Database.EFCore;

public class EFCoreContext : ApiAuthorizationDbContext<NummiUser> {
    public DbSet<Bar> HistoricalBars { get; set; } = default!;
    public DbSet<Bot> Bots { get; set; } = default!;
    public DbSet<OrderLog> Trades { get; set; } = default!;
    public DbSet<Price> HistoricalPrices { get; set; } = default!;
    public DbSet<Simulation> Simulations { get; set; } = default!;
    public DbSet<Strategy> Strategies { get; set; } = default!;
    public DbSet<StrategyTemplate> StrategyTemplates { get; set; } = default!;

    public DbSet<Blog> Blogs { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;

    public EFCoreContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        // Table Inheritance
        modelBuilder.Entity<Strategy>()
            .HasDiscriminator<string>("StrategyType")
            .HasValue<CSharpStrategy>("csharp");
        
        modelBuilder.Entity<StrategyTemplate>()
            .HasDiscriminator<string>("StrategyTemplateType")
            .HasValue<CSharpStrategyTemplate>("csharp");
        
        // Table Setups
        modelBuilder.Entity<NummiUser>().ToTable("Users");
        modelBuilder.OneToMany<NummiUser, StrategyTemplate, string>("UserId", u => u.StrategyTemplates);
        modelBuilder.OneToMany<NummiUser, Simulation, string>("UserId", u => u.Simulations)
            .IsRequired();
        modelBuilder.OneToMany<NummiUser, Bot, string>("UserId", u => u.Bots)
            .IsRequired();

        modelBuilder.Entity<Bar>().ToTable("HistoricBar");
        modelBuilder.Entity<Bar>()
            .HasKey(b => new { b.Symbol, b.OpenTime, b.Period });

        modelBuilder.Entity<Bot>().ToTable("Bot");
        modelBuilder.Entity<Bot>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Bot>()
            .HasAlternateKey("UserId", "Name");
        modelBuilder.Entity<Bot>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("BotId");
        modelBuilder.OneToMany<Bot, BotActivation>("BotId", s => s.ActivationHistory)
            .IsRequired();
        modelBuilder.OneToOneInverse<Bot, BotActivation>("CurrentBotActivationId", s => s.CurrentActivation)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<BotActivation>().ToTable("BotActivation");
        modelBuilder.Entity<BotActivation>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<BotActivation>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("BotActivationId");
        modelBuilder.OneToOne<BotActivation, Strategy>("BotActivationId", s => s.Strategy);
        modelBuilder.OneToMany<BotActivation, BotLog>("BotActivationId", s => s.Logs)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BotLog>().ToTable("BotLog");
        modelBuilder.Entity<BotLog>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<BotLog>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("BotLogId");

        modelBuilder.Entity<OrderLog>().ToTable("OrderLog");
        modelBuilder.Entity<OrderLog>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<OrderLog>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("OrderLogId");
        modelBuilder.Entity<OrderLog>()
            .Property(s => s.Quantity)
            .HasJsonConversion();

        modelBuilder.Entity<StrategyLog>().ToTable("StrategyLog");
        modelBuilder.Entity<StrategyLog>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<StrategyLog>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("StrategyLogId");
        modelBuilder.OneToMany<StrategyLog, OrderLog>("StrategyLogId", s => s.Orders);

        modelBuilder.Entity<Price>().ToTable("HistoricPrice");
        modelBuilder.Entity<Price>()
            .HasKey(b => new { b.Symbol, b.Time });
        
        modelBuilder.Entity<Simulation>().ToTable("Simulation");
        modelBuilder.Entity<Simulation>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Simulation>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("SimulationId");
        modelBuilder.OneToOne<Simulation, Strategy>("SimulationId", s => s.Strategy);

        modelBuilder.Entity<Strategy>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Strategy>()
            .Property(s => s.ParentTemplate)
            .HasJsonConversion();
        modelBuilder.Entity<Strategy>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("StrategyId");
        modelBuilder.OneToMany<Strategy, StrategyLog>("StrategyId", s => s.Logs)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StrategyTemplate>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<StrategyTemplate>()
            .HasAlternateKey(b => b.Name);
        modelBuilder.Entity<StrategyTemplate>()
            .Property<Ksuid>(b => b.Id)
            .HasColumnName("StrategyTemplateId");
        modelBuilder.Entity<CSharpStrategyTemplate>().HasData(new CSharpStrategyTemplate(
            name: "Opportunist",
            strategyTypeName: typeof(OpportunistStrategy).FullName!,
            parameterTypeName: typeof(OpportunistParameters).FullName,
            stateTypeName: typeof(OpportunistState).FullName
        ) {
            CreatedAt = DateTimeOffset.UtcNow
        });
        modelBuilder.Entity<CSharpStrategyTemplate>().HasData(new CSharpStrategyTemplate(
            name: "Opportunist2",
            strategyTypeName: typeof(OpportunistStrategy).FullName!,
            parameterTypeName: typeof(OpportunistParameters).FullName,
            stateTypeName: typeof(OpportunistState).FullName
        ) {
            CreatedAt = DateTimeOffset.Now
        });
        
        modelBuilder.OneToMany<BotLog, StrategyLog>(s => s.BotLogId);
        
        // Testing
        modelBuilder.OneToOne<Blog, Post>("BlogId", b => b.Post)
            .OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.RegisterJsonProperty<Post, Metadata>(p => p.Meta);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);
        builder.ConvertProperties<Ksuid, KsuidConverter>();
        builder.ConvertProperties<TradingMode, EnumToStringConverter<TradingMode>>();
        builder.ConvertProperties<StrategyAction, EnumToStringConverter<StrategyAction>>();
        builder.ConvertProperties<SimulationState, EnumToStringConverter<SimulationState>>();
        builder.ConvertProperties<OrderSide, EnumToStringConverter<OrderSide>>();
        builder.ConvertProperties<OrderType, EnumToStringConverter<OrderType>>();
        builder.ConvertProperties<TimeInForce, EnumToStringConverter<TimeInForce>>();
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach(var insertedEntry in insertedEntries) {
            if (insertedEntry is Audited auditableEntity) {
                auditableEntity.CreatedAt = DateTimeOffset.UtcNow;
            }
        }

        var modifiedEntries = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries) {
            if (modifiedEntry is Audited auditableEntity) {
                auditableEntity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
    
    public override int SaveChanges() {
        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach(var insertedEntry in insertedEntries) {
            if (insertedEntry is Audited auditableEntity) {
                auditableEntity.CreatedAt = DateTimeOffset.UtcNow;
            }
        }

        var modifiedEntries = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries) {
            if (modifiedEntry is Audited auditableEntity) {
                auditableEntity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        var result = base.SaveChanges();
        return result;
    }
}