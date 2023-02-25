// using Nummi.Core.Database.Common;
// using Nummi.Core.Domain.Crypto;
// using Nummi.Core.Domain.New;
// using Nummi.Core.Exceptions;
// using NummiTests.Utils;
//
// namespace NummiTests.Mocks; 
//
// public class BarTestRepository : GenericTestRepository<BarKey, Bar>, IBarRepository2 {
//
//     public Dictionary<BarKey, Bar> Database { get; } = new();
//
//     public Bar? FindById(string symbol, DateTimeOffset openTime, TimeSpan period) {
//         var key = new BarKey(symbol, openTime, period);
//         return Database.GetValueOrDefault(key);
//     }
//     
//     
//
//     public List<Bar> FindByIdRange(string symbol, DateTimeOffset startOpenTimeUtc, DateTimeOffset endOpenTimeUtc, TimeSpan period) {
//         return Database
//             .Where(e =>
//                 e.Key.Symbol == symbol
//                 && e.Key.Period == period
//                 && e.Key.OpenTimeUtc >= startOpenTimeUtc
//                 && e.Key.OpenTimeUtc <= endOpenTimeUtc
//             )
//             .Select(e => e.Value)
//             .ToList();
//     }
//
//     public Bar Add(Bar bar) {
//         var key = new BarKey(bar.Symbol, bar.OpenTime, bar.Period);
//         if (Database.ContainsKey(key)) {
//             throw new InvalidUserArgumentException($"Bar already exists {bar}");
//         }
//
//         Database[key] = bar;
//         return bar;
//     }
//
//     public void AddRange(IEnumerable<Bar> bars) {
//         foreach (var bar in bars) {
//             try {
//                 Add(bar);
//             }
//             catch (InvalidUserArgumentException) {
//             }
//         }
//     }
//
//     public void Add(IDictionary<string, List<Bar>> barDict) {
//         foreach (List<Bar> barList in barDict.Values) {
//             foreach (Bar bar in barList) {
//                 var key = new BarKey(bar.Symbol, bar.OpenTime, bar.Period);
//                 if (Database.ContainsKey(key)) {
//                     throw new InvalidUserArgumentException($"Bar already exists {bar}");
//                 }
//                 Database[key] = bar;
//             }
//         }
//     }
//
//     public void Save() {
//         
//     }
// }
//
// public class BarKey {
//     public string Symbol { get; }
//     public DateTimeOffset OpenTimeUtc { get; }
//     public TimeSpan Period { get; }
//     public BarKey(string symbol, DateTimeOffset openTime, TimeSpan period) {
//         Symbol = symbol;
//         OpenTimeUtc = openTime;
//         Period = period;
//     }
//
//     protected bool Equals(BarKey other) {
//         return Symbol == other.Symbol && OpenTimeUtc == other.OpenTimeUtc && Period == other.Period;
//     }
//
//     public override bool Equals(object? obj) {
//         if (ReferenceEquals(null, obj)) return false;
//         if (ReferenceEquals(this, obj)) return true;
//         if (obj.GetType() != this.GetType()) return false;
//         return Equals((BarKey)obj);
//     }
//
//     public override int GetHashCode() {
//         return HashCode.Combine(Symbol, OpenTimeUtc, Period);
//     }
// }