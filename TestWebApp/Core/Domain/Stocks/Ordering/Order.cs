using Alpaca.Markets;

namespace TestWebApp.Core.Domain.Stocks.Ordering; 

public class Order {
    public Guid OrderId { get; private set; }
    public string? ClientOrderId { get; private set; }
    public DateTime? CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? SubmittedAtUtc { get; private set; }
    public DateTime? FilledAtUtc { get; private set; }
    public DateTime? ExpiredAtUtc { get; private set; }
    public DateTime? CancelledAtUtc { get; private set; }
    public DateTime? FailedAtUtc { get; private set; }
    public DateTime? ReplacedAtUtc { get; private set; }
    public Guid AssetId { get; private set; }
    public string Symbol { get; private set; }
    public AssetClass AssetClass { get; private set; }
    public decimal? Notional { get; private set; }
    public decimal? Quantity { get; private set; }
    public decimal FilledQuantity { get; private set; }
    public long IntegerQuantity { get; private set; }
    public long IntegerFilledQuantity { get; private set; }
    public OrderType OrderType { get; private set; }
    public OrderClass OrderClass { get; private set; }
    public OrderSide OrderSide { get; private set; }
    public TimeInForce TimeInForce { get; private set; }
    public decimal? LimitPrice { get; private set; }
    public decimal? StopPrice { get; private set; }
    public decimal? TrailOffsetInDollars { get; private set; }
    public decimal? TrailOffsetInPercent { get; private set; }
    public decimal? HighWaterMark { get; private set; }
    public decimal? AverageFillPrice { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Guid? ReplacedByOrderId { get; private set; }
    public Guid? ReplacesOrderId { get; private set; }

    public Order(Guid orderId = default, string? clientOrderId = null, DateTime? createdAtUtc = default, DateTime? updatedAtUtc = default, DateTime? submittedAtUtc = default, DateTime? filledAtUtc = default, DateTime? expiredAtUtc = default, DateTime? cancelledAtUtc = default, DateTime? failedAtUtc = default, DateTime? replacedAtUtc = default, Guid assetId = default, string symbol = null, AssetClass assetClass = default, decimal? notional = default, decimal? quantity = default, decimal filledQuantity = default, long integerQuantity = default, long integerFilledQuantity = default, OrderType orderType = default, OrderClass orderClass = default, OrderSide orderSide = default, TimeInForce timeInForce = default, decimal? limitPrice = default, decimal? stopPrice = default, decimal? trailOffsetInDollars = default, decimal? trailOffsetInPercent = default, decimal? highWaterMark = default, decimal? averageFillPrice = default, Alpaca.Markets.OrderStatus orderStatus = default, Guid? replacedByOrderId = default, Guid? replacesOrderId = default) {
        OrderId = orderId;
        ClientOrderId = clientOrderId;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
        SubmittedAtUtc = submittedAtUtc;
        FilledAtUtc = filledAtUtc;
        ExpiredAtUtc = expiredAtUtc;
        CancelledAtUtc = cancelledAtUtc;
        FailedAtUtc = failedAtUtc;
        ReplacedAtUtc = replacedAtUtc;
        AssetId = assetId;
        Symbol = symbol;
        AssetClass = assetClass;
        Notional = notional;
        Quantity = quantity;
        FilledQuantity = filledQuantity;
        IntegerQuantity = integerQuantity;
        IntegerFilledQuantity = integerFilledQuantity;
        OrderType = orderType;
        OrderClass = orderClass;
        OrderSide = orderSide;
        TimeInForce = timeInForce;
        LimitPrice = limitPrice;
        StopPrice = stopPrice;
        TrailOffsetInDollars = trailOffsetInDollars;
        TrailOffsetInPercent = trailOffsetInPercent;
        HighWaterMark = highWaterMark;
        AverageFillPrice = averageFillPrice;
        OrderStatus = orderStatus;
        ReplacedByOrderId = replacedByOrderId;
        ReplacesOrderId = replacesOrderId;
    }

    public Order() {
        
    }
}