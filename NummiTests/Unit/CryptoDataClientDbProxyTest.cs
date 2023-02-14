using Moq;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Domain.New;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;
using NummiTests.Mocks;

namespace NummiTests.Unit; 

public class CryptoDataClientDbProxyTest {

    private Mock<BinanceClientAdapter>? binanceClientAdapter;
    private BarRepositoryMock? barRepository;
    private CryptoDataClientDbProxy? subject;

    [SetUp]
    public void SetUp() {
        barRepository = new BarRepositoryMock();
        binanceClientAdapter = new Mock<BinanceClientAdapter>();
        subject = new CryptoDataClientDbProxy(binanceClientAdapter.Object, barRepository);
    }

    [Test]
    public void GetBars_DbIsEmpty_ShouldAddAllBarsToDb() {
        var now = DateTime.UtcNow;
        var symbols = new HashSet<string> { "BTCUSD" };
        var dateRange = new DateRange(now, now + TimeSpan.FromMinutes(10));
        var period = Period.Minute;
        binanceClientAdapter!.Setup(b =>
            b.GetBars(
                It.IsAny<IDictionary<string, DateRange>>(),
                It.Is<Period>(p => p == Period.Minute)
            )).Returns(GenerateBars(symbols, dateRange, period));

        subject!.GetBars(symbols, dateRange, period);
        
        Assert.That(barRepository!.Database, Has.Count.EqualTo(11));
    }

    [Test]
    public void GetBars_DbMissingMiddleRange_ShouldAddMissingRangeBarsToDb() {
        var period = Period.Minute;
        var now = DateTime.UtcNow.Truncate(period.Time);
        var symbols = new HashSet<string> { "BTCUSD" };
        var fullRange = new DateRange(now, now + TimeSpan.FromMinutes(10));
        var partialRange = new DateRange(now + TimeSpan.FromMinutes(2), now + TimeSpan.FromMinutes(5));
        barRepository!.Add(GenerateBars(symbols, partialRange, period));
        binanceClientAdapter!.Setup(b =>
            b.GetBars(
                It.Is<IDictionary<string, DateRange>>(d => 
                    d["BTCUSD"].Start == fullRange.Start
                    && d["BTCUSD"].End == fullRange.End
                ),
                It.Is<Period>(p => p == Period.Minute)
            )).Returns(GenerateBars(symbols, fullRange, period));
        var originalDbCount = barRepository.Database.Count;

        subject!.GetBars(symbols, fullRange, period);
        
        Assert.That(barRepository!.Database, Has.Count.EqualTo(originalDbCount + 7));
        
    }

    [Test]
    public void GetBars_DbMissingStartRange_ShouldAddMissingRangeBarsToDb() {
        var period = Period.Minute;
        var now = DateTime.UtcNow.Truncate(period.Time);
        var symbols = new HashSet<string> { "BTCUSD" };
        var fullRange = new DateRange(now, now + TimeSpan.FromMinutes(10));
        var partialRange = new DateRange(now + TimeSpan.FromMinutes(5), now + TimeSpan.FromMinutes(10));
        barRepository!.Add(GenerateBars(symbols, partialRange, period));
        binanceClientAdapter!.Setup(b =>
            b.GetBars(
                It.Is<IDictionary<string, DateRange>>(d => 
                    d["BTCUSD"].Start == fullRange.Start
                    && d["BTCUSD"].End == partialRange.Start - TimeSpan.FromMinutes(1)
                ),
                It.Is<Period>(p => p == Period.Minute)
            )).Returns(GenerateBars(symbols, fullRange, period));
        var originalDbCount = barRepository.Database.Count;

        subject!.GetBars(symbols, fullRange, period);
        
        Assert.That(barRepository!.Database, Has.Count.EqualTo(originalDbCount + 5));
    }

    [Test]
    public void GetBars_3Symbols_DbMissingStartRange_ShouldAddMissingRangeBarsToDb() {
        var period = Period.Minute;
        var now = DateTime.UtcNow.Truncate(period.Time);
        var symbols = new HashSet<string> { "BTCUSD", "ETHUSD", "DOGEUSD" };
        var fullRange = new DateRange(now, now + TimeSpan.FromMinutes(9));
        var partialBTCRange = new DateRange(now + TimeSpan.FromMinutes(5), now + TimeSpan.FromMinutes(9));
        var partialETHRange = new DateRange(now + TimeSpan.FromMinutes(2), now + TimeSpan.FromMinutes(4));
        var partialDOGERange = fullRange;
        barRepository!.Add(GenerateBars("BTCUSD", partialBTCRange, period)); // 5-9
        barRepository!.Add(GenerateBars("ETHUSD", partialETHRange, period)); // 2-4
        barRepository!.Add(GenerateBars("DOGEUSD", partialDOGERange, period));

        var missingRanges = new Dictionary<string, DateRange>() {
            { "BTCUSD", new DateRange(fullRange.Start, partialBTCRange.Start - TimeSpan.FromMinutes(1)) },
            { "ETHUSD", new DateRange(fullRange.Start, fullRange.End) }
        };
        
        binanceClientAdapter!.Setup(b =>
            b.GetBars(
                It.Is<IDictionary<string, DateRange>>(d => 
                    d["BTCUSD"].Start == missingRanges["BTCUSD"].Start
                    && d["BTCUSD"].End == missingRanges["BTCUSD"].End
                    && d["ETHUSD"].Start == missingRanges["ETHUSD"].Start
                    && d["ETHUSD"].End == missingRanges["ETHUSD"].End
                ),
                It.Is<Period>(p => p == Period.Minute)
            )).Returns(GenerateBars(missingRanges, period));
        var originalDbCount = barRepository.Database.Count;

        subject!.GetBars(symbols, fullRange, period);
        
        Assert.That(barRepository!.Database, Has.Count.EqualTo(originalDbCount + 12));
    }

    private IDictionary<string, List<Bar>> GenerateBars(string symbol, DateRange dateRange, Period period) {
        return GenerateBars(new HashSet<string>() { symbol }, dateRange, period);
    }

    private IDictionary<string, List<Bar>> GenerateBars(ISet<string> symbols, DateRange dateRange, Period period) {
        return GenerateBars(symbols.ToDictionary(v => v, v => dateRange), period);
    }

    private IDictionary<string, List<Bar>> GenerateBars(IDictionary<string, DateRange> symbols, Period period) {
        var bars = new Dictionary<string, List<Bar>>();
        foreach (var (symbol, dateRange) in symbols) {
            bars[symbol] = new List<Bar>();
            var startMs = dateRange.Start.Truncate(period.Time);
            var endMs = dateRange.End.Truncate(period.Time);
            while (startMs <= endMs) {
                bars[symbol].Add(new Bar(symbol, startMs, period.Time, 10.0m, 10.0m, 10.0m, 10.0m, 10.0m));
                startMs += period.Time;
            }
        }

        return bars;
    }
    
}