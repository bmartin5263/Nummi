namespace TestWebApp.Domain.Model;

public class Trade
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public SaleType SaleType { get; set; }
}