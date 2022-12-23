using Microsoft.EntityFrameworkCore;
using TestWebApp.Domain;
using TestWebApp.Domain.Model;

namespace TestWebApp.Data;

public class MyDbContext : DbContext
{
    public DbSet<Trade> Trades { get; set; }
}