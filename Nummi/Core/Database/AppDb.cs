using Alpaca.Markets;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Nummi.Core.Domain.Stocks.Ordering;
using Nummi.Core.Domain.User;
using Nummi.Core.Domain.Stocks;

namespace Nummi.Core.Database;

public class AppDb : ApiAuthorizationDbContext<User> {
    public DbSet<Order> Orders { get; set; } = default!;
    
    public AppDb(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        
        builder.Entity<Order>()
            .Property(e => e.AssetClass)
            .HasConversion(new EnumToStringConverter<AssetClass>());
        
        builder.Entity<Order>()
            .Property(e => e.OrderType)
            .HasConversion(new EnumToStringConverter<OrderType>());
        
        builder.Entity<Order>()
            .Property(e => e.OrderClass)
            .HasConversion(new EnumToStringConverter<OrderClass>());
        
        builder.Entity<Order>()
            .Property(e => e.OrderSide)
            .HasConversion(new EnumToStringConverter<OrderSide>());
        
        builder.Entity<Order>()
            .Property(e => e.TimeInForce)
            .HasConversion(new EnumToStringConverter<TimeInForce>());
        
        builder.Entity<Order>()
            .Property(e => e.OrderStatus)
            .HasConversion(new EnumToStringConverter<OrderStatus>());
    }
}