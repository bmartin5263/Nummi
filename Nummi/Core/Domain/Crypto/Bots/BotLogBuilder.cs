// using System.Diagnostics.CodeAnalysis;
// using Nummi.Core.Domain.Crypto.Ordering;
// using Nummi.Core.Domain.Crypto.Strategies;
// using Nummi.Core.Domain.Crypto.Strategies.Log;
//
// namespace Nummi.Core.Domain.Crypto.Bots; 
//
// [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Persisted precomputed values")]
// public class BotLogBuilder {
//     
//     public Bot Bot { get; private set; }
//     public Strategy Strategy { get; private set; }
//     public decimal Funds { get; private set; }
//     public TradingMode Mode { get; private set; }
//     public BotAction Action { get; private set; }
//     public DateTime StartTime { get; private set; }
//     public int ApiCalls { get; private set; }
//     public TimeSpan TotalApiCallTime { get; private set; }
//     public Exception? Error { get; set; }
//     public IList<OrderLog> Orders { get; } = new List<OrderLog>();
//
//     public BotLogBuilder(Bot bot, BotAction action) {
//         Bot = bot;
//         Strategy = bot.Strategy!;
//         Funds = bot.Funds;
//         Mode = bot.Mode;
//         Action = action;
//         StartTime = DateTime.UtcNow;
//     }
//
//     public void LogApiCall(TimeSpan duration) {
//         ++ApiCalls;
//         TotalApiCallTime += duration;
//     }
//
//     public void LogOrder(OrderRequest order, decimal fundsBefore, decimal fundsAfter) {
//         Orders.Add(new OrderLog {
//             Symbol = order.Symbol,
//             Duration = order.Duration,
//             FundsBefore = fundsBefore,
//             FundsAfter = fundsAfter,
//             Quantity = order.Quantity,
//             Side = order.Side,
//             Type = order.Type,
//             SubmittedAt = DateTime.UtcNow
//         });
//     }
//
//     public void LogOrder(OrderRequest order, decimal fundsBefore, Exception error) {
//         Orders.Add(new OrderLog {
//             Symbol = order.Symbol,
//             Duration = order.Duration,
//             Error = error.ToString(),
//             FundsBefore = fundsBefore,
//             FundsAfter = fundsBefore,
//             Quantity = order.Quantity,
//             Side = order.Side,
//             Type = order.Type,
//             SubmittedAt = DateTime.UtcNow
//         });
//     }
//
//     public BotLog Build() {
//         return new BotLog {
//             Bot = Bot,
//             Strategy = Strategy,
//             Funds = Funds,
//             Mode = Mode,
//             Action = Action,
//             StartTime = StartTime,
//             EndTime = DateTime.UtcNow,
//             Error = Error?.ToString(),
//             ApiCalls = ApiCalls,
//             TotalApiCallTime = TotalApiCallTime,
//             Orders = Orders
//         };
//     }
// }