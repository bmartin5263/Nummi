using Nummi.Core.Domain.Crypto.Data;

namespace Nummi.Core.Domain.Crypto.Client; 

public interface ICryptoClient {

    public IDictionary<string, MinuteBar> GetMinuteBars(ISet<string> symbols);
    public HistoricalBars GetHistoricalMinuteBars(ISet<string> symbols, DateTime startTime);
    
}