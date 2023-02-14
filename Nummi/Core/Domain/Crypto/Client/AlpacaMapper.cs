using Alpaca.Markets;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.New;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using IBar = Alpaca.Markets.IBar;
using OrderSide = Nummi.Core.Domain.New.OrderSide;
using OrderType = Nummi.Core.Domain.New.OrderType;
using TimeInForce = Nummi.Core.Domain.New.TimeInForce;

namespace Nummi.Core.Domain.Crypto.Client; 

public static class AlpacaMapper {

    public static OrderType ToDomain(this Alpaca.Markets.OrderType orderType) {
        return orderType switch {
            Alpaca.Markets.OrderType.Market => OrderType.Market,
            Alpaca.Markets.OrderType.Limit => OrderType.Limit,
            Alpaca.Markets.OrderType.StopLimit => OrderType.StopLimit,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }
    
    public static Alpaca.Markets.OrderType ToAlpaca(this OrderType orderType) {
        return orderType switch {
            OrderType.Market => Alpaca.Markets.OrderType.Market,
            OrderType.Limit => Alpaca.Markets.OrderType.Limit,
            OrderType.StopLimit => Alpaca.Markets.OrderType.StopLimit,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };
    }

    public static OrderSide ToDomain(this Alpaca.Markets.OrderSide orderSide) {
        return orderSide switch {
            Alpaca.Markets.OrderSide.Buy => OrderSide.Buy,
            Alpaca.Markets.OrderSide.Sell => OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    public static Alpaca.Markets.OrderSide ToAlpaca(this OrderSide orderSide) {
        return orderSide switch {
            OrderSide.Buy => Alpaca.Markets.OrderSide.Buy,
            OrderSide.Sell => Alpaca.Markets.OrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(nameof(orderSide), orderSide, null)
        };
    }

    public static TimeInForce ToDomain(this Alpaca.Markets.TimeInForce timeInForce) {
        return timeInForce switch {
            Alpaca.Markets.TimeInForce.Gtc => TimeInForce.Gtc,
            Alpaca.Markets.TimeInForce.Ioc => TimeInForce.Ioc,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }

    public static Alpaca.Markets.TimeInForce ToAlpaca(this TimeInForce timeInForce) {
        return timeInForce switch {
            TimeInForce.Gtc => Alpaca.Markets.TimeInForce.Gtc,
            TimeInForce.Ioc => Alpaca.Markets.TimeInForce.Ioc,
            _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, null)
        };
    }

    public static OrderQuantity ToAlpaca(this CryptoOrderQuantity quantity) {
        if (quantity.Coins == null) {
            return OrderQuantity.Notional(quantity.Dollars!.Value);
        }
        if (quantity.Dollars == null) {
            return OrderQuantity.Fractional(quantity.Coins!.Value);
        }
        throw new InvalidSystemArgumentException($"{nameof(CryptoOrderQuantity)} cannot be mapped to {nameof(OrderQuantity)}");
    }

    public static Snapshot ToDomain(this ISnapshot snapshot) {
        return new Snapshot(
            symbol: snapshot.Symbol,
            quote: snapshot.Quote?.ToDomain(),
            trade: snapshot.Trade?.ToDomain(),
            minuteBar: snapshot.MinuteBar?.ToDomain(Period.Minute),
            currentDailyBar: snapshot.CurrentDailyBar?.ToDomain(Period.Minute),
            previousDailyBar: snapshot.PreviousDailyBar?.ToDomain(Period.Minute)
        );
    }
    
    public static Trade ToDomain(this ITrade trade) {
        return new Trade(
            symbol: trade.Symbol,
            timestampUtc: trade.TimestampUtc,
            price: trade.Price,
            size: trade.Size,
            tradeId: trade.TradeId,
            exchange: trade.Exchange,
            tape: trade.Tape,
            update: trade.Update,
            conditions: trade.Conditions,
            takerSide: trade.TakerSide
        );
    }

    public static Bar ToDomain(this IBar bar, Period period) {
        return new Bar(
            symbol: bar.Symbol,
            openTime: bar.TimeUtc,
            period: period.Time,
            open: bar.Open,
            high: bar.High,
            low: bar.Low,
            close: bar.Close,
            volume: bar.Volume
        );
    }

    public static Quote ToDomain(this IQuote quote) {
        return new Quote(
            symbol: quote.Symbol,
            timestampUtc: quote.TimestampUtc,
            bidExchange: quote.BidExchange,
            askExchange: quote.AskExchange,
            bidPrice: quote.BidPrice,
            askPrice: quote.AskPrice,
            bidSize: quote.BidSize,
            askSize: quote.AskSize,
            tape: quote.Tape
        );
    }

    public static NewOrderRequest ToAlpaca(this OrderRequest request) {
        return new NewOrderRequest(
            request.Symbol,
            request.Quantity.ToAlpaca(),
            request.Side.ToAlpaca(),
            request.Type.ToAlpaca(),
            request.Duration.ToAlpaca()
        );
    }

    public static Order ToDomain(this IOrder order) {
        return new Order {
            ExternalId = order.OrderId,
            SubmittedAt = order.SubmittedAtUtc ?? DateTime.UtcNow,
            Symbol = order.Symbol,
            Notional = order.Notional,
            Quantity = order.Quantity,
            OrderType = order.OrderType.ToDomain(),
            OrderSide = order.OrderSide.ToDomain(),
            TimeInForce = order.TimeInForce.ToDomain(),
            LimitPrice = order.LimitPrice,
            StopPrice = order.StopPrice,
            AverageFillPrice = order.AverageFillPrice,
            OrderStatus = order.OrderStatus
        };
    }

    // public static Order ToDomain(this IOrder request) {
    //     return new Order(
    //         request.OrderId,
    //         request.ClientOrderId,
    //         request.CreatedAtUtc,
    //         request.UpdatedAtUtc,
    //         request.SubmittedAtUtc,
    //         request.FilledAtUtc,
    //         request.ExpiredAtUtc,
    //         request.CancelledAtUtc,
    //         request.FailedAtUtc,
    //         request.ReplacedAtUtc,
    //         request.AssetId,
    //         request.Symbol,
    //         request.AssetClass,
    //         request.Notional,
    //         request.Quantity,
    //         request.FilledQuantity,
    //         request.IntegerQuantity,
    //         request.IntegerFilledQuantity,
    //         request.OrderType,
    //         request.OrderClass,
    //         request.OrderSide,
    //         request.TimeInForce,
    //         request.LimitPrice,
    //         request.StopPrice,
    //         request.TrailOffsetInDollars,
    //         request.TrailOffsetInPercent,
    //         request.HighWaterMark,
    //         request.AverageFillPrice,
    //         request.OrderStatus,
    //         request.ReplacedByOrderId,
    //         request.ReplacesOrderId
    //     );
    // }
}