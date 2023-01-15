using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy.Opportunist; 

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class OpportunistParameters {
    public string? SomeData { get; set; }
    public int? Number { get; set; }

    public override string ToString() {
        return this.ToFormattedString();
    }
}