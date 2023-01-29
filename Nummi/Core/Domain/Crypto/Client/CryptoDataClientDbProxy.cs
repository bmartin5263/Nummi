using NLog;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoDataClientDbProxy : ICryptoDataClient {
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private BinanceClientAdapter BinanceClient { get; }
    private IBarRepository BarRepository { get; }

    public CryptoDataClientDbProxy(BinanceClientAdapter binanceClient, IBarRepository barRepository) {
        BinanceClient = binanceClient;
        BarRepository = barRepository;
    }

    public IDictionary<string, List<Bar>> GetBars(ISet<string> symbols, DateRange dateRange, Period period) {
        Log.Info($"GetMinuteBars Input DateRange: {dateRange.ToString().Yellow()}");
        dateRange = dateRange.Truncate(period.Time);
        Log.Info($"GetMinuteBars Truncated DateRange: {dateRange.ToString().Yellow()}");
        
        IDictionary<string, DbBars> dbBars = GetBarsFromDb(symbols, dateRange, period);
        Dictionary<string, List<Bar>> result = new();
        Dictionary<string, DateRange> missingRanges = new();

        foreach ((string symbol, DbBars bars) in dbBars) {
            if (bars.MissingRange == null) {
                result[symbol] = bars.Bars;
            }
            else {
                missingRanges[symbol] = bars.MissingRange!.Value;
                var resultList = new List<Bar>();
                resultList.AddRange(bars.Bars);
                result[symbol] = resultList;
            }
        }

        if (missingRanges.Count == 0) {
            return result;
        }

        IDictionary<string, List<Bar>> clientBars = BinanceClient.GetBars(missingRanges, period);
        
        var allBars = clientBars.SelectMany(v => v.Value);
        var dbRowsAdded = BarRepository.AddRange(allBars);

        if (dbRowsAdded > 0) {
            Log.Info($"Inserted {dbRowsAdded.ToString().Green()} Bars into DB");
            BarRepository.Save();
        }
        
        foreach ((string symbol, List<Bar> bars) in clientBars) {
            if (!result.ContainsKey(symbol)) {
                result[symbol] = new List<Bar>();
            }
            result[symbol].AddRange(bars);
            result[symbol].Sort();
        }
        
        return result;
    }

    private IDictionary<string, DbBars> GetBarsFromDb(ISet<string> symbols, DateRange dateRange, Period period) {
        long periodUnixMs = period.UnixMs;
        long startUnixMs = dateRange.Start.ToUnixTimeMs();
        long endUnixMs = dateRange.End.ToUnixTimeMs();
        
        var preloadedBars = new Dictionary<string, DbBars>();
        foreach (var symbol in symbols) {
            var bars = BarRepository.FindByIdRange(symbol, startUnixMs, endUnixMs, periodUnixMs);

            if (bars.Count == 0) {
                // No bars preloaded, missing range is the entire input range
                preloadedBars[symbol] = new DbBars(new List<Bar>(), dateRange);
                continue;
            }

            // Check both ends to see if any part of the range is missing
            long newStart = startUnixMs;
            foreach (var bar in bars) {
                if (bar.OpenTimeUnixMs == newStart) {
                    newStart += periodUnixMs;
                }
                else {
                    break;
                }
            }

            if (newStart > endUnixMs) {
                // Got the entire range, no additional API calls needed
                preloadedBars[symbol] = new DbBars(bars);
                continue;
            }

            long newEnd = endUnixMs;
            for (int i = bars.Count - 1; i >= 0; --i) {
                var bar = bars[i];
                if (bar.OpenTimeUnixMs == newEnd) {
                    newEnd -= periodUnixMs;
                }
                else {
                    break;
                }
            }
            
            // Got some of the range
            preloadedBars[symbol] = new DbBars(bars, new DateRange(newStart.ToUtcDateTime(), newEnd.ToUtcDateTime()));
        }

        return preloadedBars;
    }
}

class DbBars {
    public List<Bar> Bars { get; }
    public DateRange? MissingRange { get; }

    public DbBars(List<Bar>? bars = null, DateRange? missingRange = null) {
        Bars = bars ?? new List<Bar>();
        MissingRange = missingRange;
    }
}