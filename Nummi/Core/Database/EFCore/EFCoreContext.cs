using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.User;
using Nummi.Core.Domain.Simulations;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.Test;
using Nummi.Core.Exceptions;

namespace Nummi.Core.Database.EFCore;

public class EFCoreContext : 
    IdentityDbContext<NummiUser, NummiRole, Ksuid, NummiUserClaim, NummiUserRole, NummiUserLogin, NummiRoleClaim, NummiUserToken>,
    IPersistedGrantDbContext 
{
    public DbSet<PersistedGrant> PersistedGrants { get; set; } = default!;
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; } = default!;
    public DbSet<Key> Keys { get; set; } = default!;
    public DbSet<Bar> HistoricalBars { get; set; } = default!;
    public DbSet<Bot> Bots { get; set; } = default!;
    public DbSet<OrderLog> Trades { get; set; } = default!;
    public DbSet<Price> HistoricalPrices { get; set; } = default!;
    public DbSet<Simulation> Simulations { get; set; } = default!;
    public DbSet<Strategy> Strategies { get; set; } = default!;
    public DbSet<StrategyTemplate> StrategyTemplates { get; set; } = default!;

    public DbSet<Blog> Blogs { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

    public EFCoreContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options)
    {
        _operationalStoreOptions = operationalStoreOptions;
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);

        // Table Inheritance
        builder.Entity<Strategy>()
            .HasDiscriminator<string>("StrategyType")
            .HasValue<StrategyBuiltin>("builtin");
        
        builder.Entity<StrategyTemplateVersion>()
            .HasDiscriminator<string>("StrategyTemplateType")
            .HasValue<StrategyTemplateVersionBuiltin>("builtin");
        
        // Table Setups
        builder.Entity<NummiUser>().ToTable("User");
        builder.OneToMany<NummiUser, StrategyTemplate, Ksuid>(t => t.UserId, u => u.StrategyTemplates);
        builder.OneToMany<NummiUser, Simulation, Ksuid>("UserId", u => u.Simulations)
            .IsRequired();
        builder.OneToMany<NummiUser, Bot, Ksuid>("UserId", u => u.Bots)
            .IsRequired();
        
        builder.Entity<NummiRole>().ToTable("Role");
        builder.Entity<NummiRoleClaim>().ToTable("RoleClaim");
        builder.Entity<NummiUserClaim>().ToTable("UserClaim");
        builder.Entity<NummiUserLogin>().ToTable("UserLogin");
        builder.Entity<NummiUserRole>().ToTable("UserRole");
        builder.Entity<NummiUserToken>().ToTable("UserToken");

        builder.DefineTable<Bar>("HistoricalBar", b => new { b.Symbol, b.OpenTime, b.Period });
        
        builder.DefineTable<Bot>();
        builder.Entity<Bot>().HasAlternateKey("UserId", "Name");
        builder.OneToMany<Bot, BotActivation>("BotId", s => s.ActivationHistory)
            .IsRequired();
        builder.OneToOneOptionalInverse<Bot, BotActivation>("CurrentBotActivationId", s => s.CurrentActivation)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.DefineTable<BotActivation>();
        builder.OneToOne<BotActivation, Strategy>("BotActivationId", s => s.Strategy);
        builder.OneToMany<BotActivation, BotLog>("BotActivationId", s => s.Logs)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.DefineTable<BotLog>();

        builder.DefineTable<OrderLog>();
        builder.Entity<OrderLog>()
            .Property(s => s.Quantity)
            .HasJsonConversion();

        builder.DefineTable<StrategyLog>();
        builder.OneToMany<StrategyLog, OrderLog>("StrategyLogId", s => s.Orders);

        builder.DefineTable<Price>("HistoricalPrice", b => new { b.Symbol, b.Time });
        
        builder.DefineTable<Simulation>();
        builder.OneToOne<Simulation, Strategy>("SimulationId", s => s.Strategy);

        builder.DefineTable<Strategy>();
        builder.Entity<Strategy>()
            .Property(s => s.ParametersJson)
            .HasColumnType("jsonb");
        builder.Entity<Strategy>()
            .Property(s => s.StateJson)
            .HasColumnType("jsonb");
        builder.OneToMany<Strategy, StrategyLog>("StrategyId", s => s.Logs)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder.ManyToOne<Strategy, StrategyTemplateVersion>("StrategyTemplateVersionId", s => s.ParentTemplateVersion)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.DefineTable<StrategyTemplate>();
        builder.Entity<StrategyTemplate>().HasAlternateKey(b => new { b.UserId, b.Name });    // User cannot have two strategies with same name
        builder.OneToMany<StrategyTemplate, StrategyTemplateVersion>("StrategyTemplateId", u => u.Versions)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.DefineTable<StrategyTemplateVersion>();
        builder.Entity<StrategyTemplateVersion>().HasAlternateKey("StrategyTemplateId", "VersionNumber");
        builder.OneToMany<BotLog, StrategyLog>(s => s.BotLogId);
        
        // Testing
        builder.OneToOne<Blog, Post>("BlogId", b => b.Post)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.RegisterJsonProperty<Post, Metadata>(p => p.Meta);
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

        var modifiedEntries = ChangeTracker.Entries()
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

        var modifiedEntries = ChangeTracker.Entries()
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
    
    private string GetEnvironmentVariable(string name) {
        return Environment.GetEnvironmentVariable(name) ??
               throw new SystemArgumentException($"Missing Env Var: {name}");
    }
}