using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.Data;
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
            OpenTimeUtc = bar.OpenTime,
            CloseTimeUtc = bar.OpenTime + bar.Period,
            Open = bar.Open,
            High = bar.High,
            Low = bar.Low,
            Close = bar.Close,
            Volume = bar.Volume
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
            Mode = bot.Mode,
            InErrorState = bot.InErrorState,
            Funds = bot.Funds,
        };
    }

    public static StrategyDto ToDto(this Strategy strategy) {
        var dto = new StrategyDto {
            Id = strategy.Id.ToString(),
            Frequency = strategy.Frequency
        };

        return dto;
    }

    public static SimulationDto ToDto(this Simulation result) {
        var dto = new SimulationDto {
        };
        return dto;
    }

    public static BotActivationDto ToDto(this BotActivation activation) {
        var dto = new BotActivationDto { };
        return dto;
    }

    public static StrategyLogDto ToDto(this StrategyLog log) {
        var dto = new StrategyLogDto {
            Id = log.Id.ToString(),
            Environment = log.Mode,
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

    public static NummiUserDto ToDto(this NummiUser user) {
        return new NummiUserDto {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            AlpacaPaperUserId = user.AlpacaPaperUserId,
            AlpacaPaperSecret = user.AlpacaPaperSecret,
            AlpacaLiveUserId = user.AlpacaLiveUserId,
            AlpacaLiveSecret = user.AlpacaLiveSecret,
            Bots = user.Bots.Select(ToDto).ToList(),
            Simulations = user.Simulations.Select(ToDto).ToList()
        };
    }
}