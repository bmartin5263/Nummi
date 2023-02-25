using Alpaca.Markets;

namespace Nummi.Core.Domain.Crypto;

public class Order {
    public required Guid ExternalId { get; init; }
    public required DateTime SubmittedAt { get; init; }
    public required string Symbol { get; init; }
    public decimal? Notional { get; init; }
    public decimal? Quantity { get; init; }
    public required OrderType OrderType { get; init; }
    public required OrderSide OrderSide { get; init; }
    public required TimeInForce TimeInForce { get; init; }
    public decimal? LimitPrice { get; init; }
    public decimal? StopPrice { get; init; }
    public decimal? AverageFillPrice { get; init; }
    public required OrderStatus OrderStatus { get; init; }
}