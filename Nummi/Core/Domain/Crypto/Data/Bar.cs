// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
// using Nummi.Core.Util;
//
// namespace Nummi.Core.Domain.Crypto.Data; 
//
// [Table("Historical" + nameof(Bar))]
// [PrimaryKey(nameof(Symbol), nameof(OpenTimeUtc), nameof(Period))]
// public class Bar : IBar, IComparable<Bar> {
//
//     public string Symbol { get; private init; }
//     public DateTime OpenTimeUtc { get; private init; }
//     public TimeSpan Period { get; private init; }
//     public decimal Open { get; private init; }
//     public decimal High { get; private init; }
//     public decimal Low { get; private init; }
//     public decimal Close { get; private init; }
//     public decimal Volume { get; private init; }
//
//     public Bar(string symbol, DateTime openTimeUtc, TimeSpan period, decimal open, decimal high, decimal low, decimal close, decimal volume) {
//         Symbol = symbol;
//         OpenTimeUtc = openTimeUtc.AssertIsUtc();
//         Period = period;
//         Open = open;
//         High = high;
//         Low = low;
//         Close = close;
//         Volume = volume;
//     }
//     
//     protected bool Equals(Bar other) {
//         return Symbol == other.Symbol 
//                && OpenTimeUtc == other.OpenTimeUtc
//                && Period == other.Period;
//     }
//
//     public int CompareTo(Bar? other) {
//         return OpenTimeUtc.CompareTo(other?.OpenTimeUtc);
//     }
//
//     public override bool Equals(object? obj) {
//         if (ReferenceEquals(null, obj)) return false;
//         if (ReferenceEquals(this, obj)) return true;
//         if (obj.GetType() != this.GetType()) return false;
//         return Equals((Bar)obj);
//     }
//
//     public override int GetHashCode() {
//         return HashCode.Combine(Symbol, OpenTimeUtc, Period);
//     }
//
//     public override string ToString() {
//         return $"{nameof(Symbol)}: {Symbol}, OpenTimeUtc: {OpenTimeUtc}, Period: {Period}, {nameof(Open)}: {Open}, {nameof(High)}: {High}, {nameof(Low)}: {Low}, {nameof(Close)}: {Close}, {nameof(Volume)}: {Volume}";
//     }
// }