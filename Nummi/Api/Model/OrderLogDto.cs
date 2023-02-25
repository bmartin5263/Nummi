using Nummi.Core.Domain.Crypto;

namespace Nummi.Api.Model; 

public class OrderLogDto {
    public string? Id { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string? Symbol { get; set; }
    public CryptoOrderQuantity? Quantity { get; set; }
    public OrderSide? Side { get; set; }
    public OrderType? Type { get; set; }
    public TimeInForce? Duration { get; set; }
    public decimal? FundsBefore { get; set; }
    public decimal? FundsAfter { get; set; }
    public string? Error { get; set; }
}