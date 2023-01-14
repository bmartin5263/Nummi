using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Crypto.Trading.Strategy.Opportunist; 

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class OpportunistState {
    public string AnotherString { get; set; }
    public int WowANumber { get; set; }

    public OpportunistState(string anotherString, int wowANumber) {
        AnotherString = anotherString;
        WowANumber = wowANumber;
    }
}