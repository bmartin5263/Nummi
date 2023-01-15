using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy.Opportunist; 

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class OpportunistState {
    public int Counter { get; set; }

    public OpportunistState(int counter) {
        Counter = counter;
    }
    
    public override string ToString() {
        return this.ToFormattedString();
    }
}