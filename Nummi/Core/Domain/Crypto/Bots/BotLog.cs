// using System.ComponentModel.DataAnnotations.Schema;
// using System.Diagnostics.CodeAnalysis;
// using System.Text.Json.Serialization;
// using KSUID;
// using Microsoft.EntityFrameworkCore;
// using Nummi.Core.Domain.Crypto.Strategies;
// using Nummi.Core.Domain.Crypto.Strategies.Log;
// using Nummi.Core.Util;
//
// namespace Nummi.Core.Domain.Crypto.Bots; 
//
// [PrimaryKey(nameof(Id))]
// [Table(nameof(BotLog))]
// public class BotLog : IComparable<BotLog> {
//     
//     public string Id { get; } = Ksuid.Generate().ToString();
//
//     [JsonIgnore]
//     public Bot Bot { get; init; } = default!;
//     
//     [JsonIgnore]
//     public Strategy Strategy { get; init; } = default!;
//     
//     public required decimal Funds { get; init; }
//     
//     [JsonConverter(typeof(JsonStringEnumConverter))]
//     public required TradingMode Mode { get; init; }
//     
//     [JsonConverter(typeof(JsonStringEnumConverter))]
//     public required BotAction Action { get; init; }
//
//     public required DateTime StartTime { get; init; }
//     
//     public required DateTime EndTime { get; init; }
//
//     [Column(nameof(TotalTime))]
//     [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
//     public TimeSpan TotalTime {
//         get => EndTime - StartTime;
//         private init { }
//     }
//     
//     public required int ApiCalls { get; init; }
//     
//     public required TimeSpan TotalApiCallTime { get; init; }
//
//     public string? Error { get; init; }
//
//     public IList<OrderLog> Orders { get; init; } = new List<OrderLog>();
//
//     public int CompareTo(BotLog? other) {
//         return StartTime.CompareTo(other?.StartTime);
//     }
//
//     public override string ToString() {
//         return this.ToFormattedString();
//     }
// }
//
// public enum BotAction {
//     Initializing, Trading
// }