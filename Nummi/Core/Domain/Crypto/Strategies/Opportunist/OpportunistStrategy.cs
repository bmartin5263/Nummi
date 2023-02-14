// using System.ComponentModel.DataAnnotations.Schema;
// using Nummi.Core.Domain.Common;
// using Nummi.Core.Exceptions;
// using Nummi.Core.External.Binance;
// using Nummi.Core.Util;
//
// namespace Nummi.Core.Domain.Crypto.Strategies.Opportunist; 
//
// public class OpportunistStrategy : Strategy, IParameterizedStrategy<OpportunistParameters> {
//
//     public ISet<string> Symbols { get; private set; } = new HashSet<string>();
//     public Type ParameterObjectType => typeof(OpportunistParameters);
//     
//     [NotMapped]
//     public OpportunistParameters Parameters {
//         get => new() {
//                 Symbols = Symbols 
//         };
//         set => Symbols = value.Symbols;
//     }
//
//     public OpportunistStrategy() 
//         : base(TimeSpan.FromMinutes(1)) 
//     {
//     }
//
//     protected override void Initialize(TradingContextAudited ctx) {
//         if (Symbols == null) {
//             throw new InvalidUserArgumentException("Symbols cannot be null");
//         }
//
//         var now = ctx.Clock.NowUtc;
//         Message($"Initializing (nowUtc={now.ToString().Yellow()} now={now.ToLocalTime().ToString().Yellow()})");
//         var bars = ctx.GetBars(
//             symbols: Symbols, 
//             dateRange: new DateRange(now - TimeSpan.FromMinutes(60), now), 
//             Period.Second
//         );
//         
//         foreach (var symbol in Symbols) {
//             var symbolBars = bars[symbol];
//             DateTime minDate = symbolBars[0].OpenTimeUtc;
//             DateTime maxDate = symbolBars[^1].OpenTimeUtc;
//             Message($"{symbol.Red()}: {minDate.ToLocalTime().ToString().Yellow()} - {maxDate.ToLocalTime().ToString().Yellow()}");
//         }
//     }
//     
//     protected override void CheckForTrades(TradingContextAudited ctx) {
//         if (Symbols == null) {
//             throw new InvalidUserArgumentException("Symbols cannot be null");
//         }
//         Message($"Checking For Trades: {ctx.Clock.Now.ToString().Yellow()} / {ctx.Clock.NowUtc.ToString().Yellow()}");
//     }
// }
//
// public class OpportunistParameters {
//     public ISet<string> Symbols { get; init; } = new HashSet<string>();
//     public override string ToString() => this.ToFormattedString();
// }