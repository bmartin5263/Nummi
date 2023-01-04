using Nummi.Core.Domain.Stocks;
using Nummi.Core.Domain.Stocks.Data;


namespace Nummi.Api.Model; 

public static class ModelMapper {
    public static SnapshotDto ToDto(this Snapshot snapshot) {
        return new SnapshotDto {
            Symbol = snapshot.Symbol,
            Quote = snapshot.Quote?.ToDto(),
            Trade = snapshot.Trade?.ToDto(),
            MinuteBar = snapshot.MinuteBar?.ToDto(),
            CurrentDailyBar = snapshot.CurrentDailyBar?.ToDto(),
            PreviousDailyBar = snapshot.PreviousDailyBar?.ToDto()
        };
    }
    
    public static TradeDto ToDto(this Trade trade) {
        return new TradeDto {
            Symbol = trade.Symbol,
            TimestampUtc = trade.TimestampUtc,
            Price = trade.Price,
            Size = trade.Size,
            TradeId = trade.TradeId,
            Exchange = trade.Exchange,
            Tape = trade.Tape,
            Update = trade.Update,
            Conditions = trade.Conditions,
            TakerSide = trade.TakerSide
        };
    }

    public static BarDto ToDto(this Bar bar) {
        return new BarDto {
            Symbol = bar.Symbol,
            TimeUtc = bar.TimeUtc,
            Open = bar.Open,
            High = bar.High,
            Low = bar.Low,
            Close = bar.Close,
            Volume = bar.Volume,
            Vwap = bar.Vwap,
            TradeCount = bar.TradeCount
        };
    }

    public static QuoteDto ToDto(this Quote quote) {
        return new QuoteDto {
            Symbol = quote.Symbol,
            TimestampUtc = quote.TimestampUtc,
            BidExchange = quote.BidExchange,
            AskExchange = quote.AskExchange,
            BidPrice = quote.BidPrice,
            AskPrice = quote.AskPrice,
            BidSize = quote.BidSize,
            AskSize = quote.AskSize,
            Tape = quote.Tape
        };
    }
}