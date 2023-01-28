using Alpaca.Markets;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Ordering;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;
using IBar = Alpaca.Markets.IBar;

namespace Nummi.Core.Domain.Crypto.Client; 

public static class AlpacaMapper {

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
            openTimeUnixMs: bar.TimeUtc.ToUnixTimeMs(),
            periodMs: period.UnixMs,
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

    public static NewOrderRequest ToAlpaca(this PlaceOrderRq request) {
        return new NewOrderRequest(
            request.Symbol,
            request.Quantity,
            OrderSide.Buy,
            OrderType.Market,
            TimeInForce.Cls
        );
    }

    public static Order ToDomain(this IOrder request) {
        return new Order(
            request.OrderId,
            request.ClientOrderId,
            request.CreatedAtUtc,
            request.UpdatedAtUtc,
            request.SubmittedAtUtc,
            request.FilledAtUtc,
            request.ExpiredAtUtc,
            request.CancelledAtUtc,
            request.FailedAtUtc,
            request.ReplacedAtUtc,
            request.AssetId,
            request.Symbol,
            request.AssetClass,
            request.Notional,
            request.Quantity,
            request.FilledQuantity,
            request.IntegerQuantity,
            request.IntegerFilledQuantity,
            request.OrderType,
            request.OrderClass,
            request.OrderSide,
            request.TimeInForce,
            request.LimitPrice,
            request.StopPrice,
            request.TrailOffsetInDollars,
            request.TrailOffsetInPercent,
            request.HighWaterMark,
            request.AverageFillPrice,
            request.OrderStatus,
            request.ReplacedByOrderId,
            request.ReplacesOrderId
        );
    }
}