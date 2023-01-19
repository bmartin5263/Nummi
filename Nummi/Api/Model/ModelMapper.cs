using Nummi.Core.Domain.Crypto.Bots;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.Crypto.Strategies;
using Nummi.Core.Domain.Test;

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

    public static BotDto ToDto(this Bot bot) {
        return new BotDto {
            Id = bot.Id.ToString(),
            Name = bot.Name,
            Environment = bot.Environment,
            InErrorState = bot.InErrorState,
            Funds = bot.Funds,
            LastStrategyLog = bot.LastStrategyLog?.ToDto(),
            Strategy = bot.Strategy?.ToDto()
        };
    }

    public static StrategyDto ToDto(this Strategy strategy) {
        var dto = new StrategyDto {
            Id = strategy.Id,
            Frequency = strategy.Frequency,
            Initialized = strategy.Initialized,
            Profit = strategy.Profit,
            TimesExecuted = strategy.TimesExecuted,
            LastExecutedAt = strategy.LastExecutedAt,
            TimesFailed = strategy.TimesFailed,
            Logs = strategy.Logs.Select(ToDto).ToList()
        };
        if (strategy is IParameterizedStrategy parameterizedStrategy) {
            dto.Parameters = parameterizedStrategy.Parameters;
        }

        return dto;
    }

    public static StrategyLogDto ToDto(this StrategyLog log) {
        var dto = new StrategyLogDto {
            Id = log.Id,
            StrategyId = log.Strategy.Id,
            Environment = log.Environment,
            StartTime = log.StartTime,
            EndTime = log.EndTime,
            Error = log.Error
        };
        return dto;
    }

    public static BlogDto ToDto(this Blog blog) {
        return new BlogDto {
            Id = blog.Id.ToString(),
            Name = blog.Name,
            Post = blog.Post?.ToDto()
        };
    }

    public static PostDto ToDto(this Post post) {
        return new PostDto {
            Id = post.Id.ToString(),
            Content = post.Content
        };
    }
}