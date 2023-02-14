namespace Nummi.Core.Domain.New; 

public class StrategyLogBuilder {
    
    public string? BotId { get; private set; }
    public Strategy Strategy { get; private set; }
    public TradingMode Mode { get; private set; }
    public StrategyAction Action { get; private set; }
    public DateTime StartTime { get; private set; }
    public int ApiCalls { get; private set; }
    public TimeSpan TotalApiCallTime { get; private set; }
    public Exception? Error { get; set; }
    public IList<OrderLog> Orders { get; } = new List<OrderLog>();

    public StrategyLogBuilder(Strategy strategy, TradingMode mode, StrategyAction action, string? botId) {
        BotId = botId;
        Strategy = strategy;
        Mode = mode;
        Action = action;
        StartTime = DateTime.UtcNow;
    }

    public void LogApiCall(TimeSpan duration) {
        ++ApiCalls;
        TotalApiCallTime += duration;
    }

    public void LogOrder(OrderRequest order, decimal fundsBefore, decimal fundsAfter) {
        Orders.Add(new OrderLog {
            Symbol = order.Symbol,
            Duration = order.Duration,
            FundsBefore = fundsBefore,
            FundsAfter = fundsAfter,
            Quantity = order.Quantity,
            Side = order.Side,
            Type = order.Type,
            SubmittedAt = DateTime.UtcNow
        });
    }

    public void LogOrder(OrderRequest order, decimal fundsBefore, Exception error) {
        Orders.Add(new OrderLog {
            Symbol = order.Symbol,
            Duration = order.Duration,
            Error = error.ToString(),
            FundsBefore = fundsBefore,
            FundsAfter = fundsBefore,
            Quantity = order.Quantity,
            Side = order.Side,
            Type = order.Type,
            SubmittedAt = DateTime.UtcNow
        });
    }

    public StrategyLog Build() {
        return new StrategyLog {
            Mode = Mode,
            Action = Action,
            StartTime = StartTime,
            EndTime = DateTime.UtcNow,
            Error = Error?.ToString(),
            ApiCalls = ApiCalls,
            TotalApiCallTime = TotalApiCallTime,
            Orders = Orders
        };
    }
}