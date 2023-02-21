using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.Data;
using Nummi.Core.Domain.New.User;
using Nummi.Core.Domain.Strategies;
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
            CreatedAt = strategy.CreatedAt,
            DeletedAt = strategy.DeletedAt,
            Logs = strategy.Logs.Select(l => l.ToDto()).ToList(),
            ParametersJson = strategy.ParametersJson,
            ParentTemplate = strategy.ParentTemplate.ToDto(),
            StateJson = strategy.StateJson,
            UpdatedAt = strategy.UpdatedAt
        };
        return dto;
    }

    public static SimulationDto ToDto(this Simulation simulation) {
        return new SimulationDto {
            Id = simulation.Id.ToString(),
            CreatedAt = simulation.CreatedAt,
            UpdatedAt = simulation.UpdatedAt,
            DeletedAt = simulation.DeletedAt,
            Error = simulation.Error,
            FinishedAt = simulation.FinishedAt,
            SimulationEndDate = simulation.SimulationEndDate,
            SimulationStartDate = simulation.SimulationStartDate,
            StartedAt = simulation.StartedAt,
            State = simulation.State,
            StrategyId = simulation.Strategy.Id.ToString(),
            TotalExecutionTime = simulation.TotalExecutionTime
        };
    }

    public static BotActivationDto ToDto(this BotActivation activation) {
        var dto = new BotActivationDto {
            Id = activation.Id.ToString(),
            CreatedAt = activation.CreatedAt,
            DeletedAt = activation.DeletedAt,
            UpdatedAt = activation.UpdatedAt,
            Strategy = activation.Strategy.ToDto(),
            Mode = activation.Mode,
            Logs = activation.Logs.Select(v => v.ToDto()).ToList()
        };
        return dto;
    }

    public static BotLogDto ToDto(this BotLog botLog) {
        var dto = new BotLogDto {
            Id = botLog.Id.ToString(),
            StartTime = botLog.StartTime,
            EndTime = botLog.EndTime,
            TotalTime = botLog.TotalTime,
            Error = botLog.Error
        };
        return dto;
    }

    public static StrategyLogDto ToDto(this StrategyLog log) {
        var dto = new StrategyLogDto {
            Id = log.Id.ToString(),
            BotLogId = log.BotLogId?.ToString(),
            Mode = log.Mode,
            Action = log.Action,
            StartTime = log.StartTime,
            EndTime = log.EndTime,
            TotalTime = log.TotalTime,
            ApiCalls = log.ApiCalls,
            TotalApiCallTime = log.TotalApiCallTime,
            Error = log.Error,
            Orders = log.Orders.Select(v => v.ToDto()).ToList()
        };
        return dto;
    }

    public static OrderLogDto ToDto(this OrderLog log) {
        var dto = new OrderLogDto {
            Id = log.Id.ToString(),
            SubmittedAt = log.SubmittedAt,
            Symbol = log.Symbol,
            Quantity = log.Quantity,
            Side = log.Side,
            Type = log.Type,
            Duration = log.Duration,
            FundsBefore = log.FundsBefore,
            FundsAfter = log.FundsAfter,
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
            Id = user.Id.ToString(),
            Username = user.UserName,
            Email = user.Email,
            Bots = user.Bots.Select(ToDto).ToList(),
            Simulations = user.Simulations.Select(ToDto).ToList()
        };
    }

    public static StrategyTemplateDto ToDto(this StrategyTemplate template) {
        return new StrategyTemplateDto {
            Id = template.Id.ToString(),
            CreatedAt = template.CreatedAt,
            DeletedAt = template.DeletedAt,
            Name = template.Name,
            UpdatedAt = template.UpdatedAt
        };
    }

    public static StrategyTemplateVerionDto ToDto(this StrategyTemplateVersion version) {
        return new StrategyTemplateVerionDto {
            VersionNumber = version.VersionNumber,
            CreatedAt = version.CreatedAt,
            DeletedAt = version.DeletedAt,
            Frequency = version.Frequency,
            Name = version.Name,
            UpdatedAt = version.UpdatedAt
        };
    }
}