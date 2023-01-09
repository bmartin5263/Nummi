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
    public DbSet<StockBot> Bots { get; set; } = default!;
    
    public AppDb(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<StockBot>()
            .Property(e => e.Strategy)
            .HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<ITradingStrategy>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        
        modelBuilder
            .Entity<StockBot>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ksuid.FromString(v)
            );
    }
}