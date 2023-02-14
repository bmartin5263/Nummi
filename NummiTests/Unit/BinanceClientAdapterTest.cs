using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;
using NummiTests.Mocks;

namespace NummiTests.Unit; 

public class BinanceClientAdapterTest {

    private BinanceClientMock? binanceClient;
    private BinanceClientAdapter? subject;

    [SetUp]
    public void SetUp() {
        var exchangeInfo = new ExchangeInfo {
            RateLimits = new[] {
                new RateLimit {
                    Interval = "MINUTE",
                    IntervalNum = 1,
                    Limit = 1000,
                    RateLimitType = "REQUEST_WEIGHT"
                }
            }
        };
        binanceClient = new BinanceClientMock();
        subject = new BinanceClientAdapter(binanceClient, exchangeInfo);
    }

    [Test]
    public void GetExchangeInfo_ShouldReturnInfo() {
        var result = subject!.GetExchangeInfo();
        var limits = result.RateLimits.ToList();
        
        Assert.That(limits, Has.Count.EqualTo(1));
        Assert.That(limits[0].Interval, Is.EqualTo("MINUTE"));
        Assert.That(limits[0].IntervalNum, Is.EqualTo(1));
        Assert.That(limits[0].Limit, Is.EqualTo(1000));
        Assert.That(limits[0].RateLimitType, Is.EqualTo("REQUEST_WEIGHT"));
        
        Assert.That(binanceClient!.GetExchangeInfoCalls, Has.Count.EqualTo(1));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(10));
    }

    [Test]
    public void GetBar_Minute_WithOneSymbol_ShouldSucceed() {
        var now = DateTimeOffset.UtcNow;
        var nowTruncated = now.Truncate(Period.Minute.Time);
        var result = subject!.GetBar(new HashSet<string> {"BTCUSD"}, now, Period.Minute);
        
        Assert.That(result["BTCUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["BTCUSD"].Period, Is.EqualTo(Period.Minute.Time));
        
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(new GetKlinesCall(
            "BTCUSD", now, now, Period.Minute, 1
        )));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(1));
    }

    [Test]
    public void GetBar_Minute_WithThreeSymbols_ShouldSucceed() {
        var now = DateTimeOffset.UtcNow;
        var nowTruncated = now.Truncate(Period.Minute.Time);
        var result = subject!.GetBar(new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD"}, now, Period.Minute);
        
        Assert.That(result["BTCUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["BTCUSD"].Period, Is.EqualTo(Period.Minute.Time));
        Assert.That(result["ETHUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["ETHUSD"].Period, Is.EqualTo(Period.Minute.Time));
        Assert.That(result["DOGEUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["DOGEUSD"].Period, Is.EqualTo(Period.Minute.Time));
        
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("BTCUSD", now, now, Period.Minute, 1)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("DOGEUSD", now, now, Period.Minute, 1)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("ETHUSD", now, now, Period.Minute, 1)
        ));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(3));
    }

    [Test]
    public void GetBar_Second_WithOneSymbol_ShouldSucceed() {
        var now = DateTimeOffset.UtcNow;
        var nowTruncated = now.Truncate(Period.Second.Time);
        var result = subject!.GetBar(new HashSet<string> {"BTCUSD"}, now, Period.Second);
        
        Assert.That(result["BTCUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["BTCUSD"].Period, Is.EqualTo(Period.Second.Time));
        
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(new GetKlinesCall(
            "BTCUSD", now, now, Period.Second, 1
        )));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(1));
    }

    [Test]
    public void GetBar_Second_WithThreeSymbols_ShouldSucceed() {
        var now = DateTimeOffset.UtcNow;
        var nowTruncated = now.Truncate(Period.Second.Time);
        var result = subject!.GetBar(new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD"}, now, Period.Second);
        
        Assert.That(result["BTCUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["BTCUSD"].Period, Is.EqualTo(Period.Second.Time));
        Assert.That(result["ETHUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["ETHUSD"].Period, Is.EqualTo(Period.Second.Time));
        Assert.That(result["DOGEUSD"].OpenTime, Is.EqualTo(nowTruncated));
        Assert.That(result["DOGEUSD"].Period, Is.EqualTo(Period.Second.Time));
        
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("BTCUSD", now, now, Period.Second, 1)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("DOGEUSD", now, now, Period.Second, 1)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("ETHUSD", now, now, Period.Second, 1)
        ));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(3));
    }

    [Test]
    public void GetBars_Minute_WithThreeSymbols_Each20MinuteRange_ShouldSucceed() {
        ISet<string> symbols = new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD"};
        DateTimeOffset start = DateTimeOffset.UtcNow;
        DateRange dateRange = new DateRange(start, start + TimeSpan.FromMinutes(19));
        var result = subject!.GetBars(symbols, dateRange, Period.Minute);
        
        foreach (var symbol in symbols) {
            Assert.That(result[symbol], Has.Count.EqualTo(20));
            DateTimeOffset runningStart = start.Truncate(Period.Minute.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Minute.Time));
                runningStart += TimeSpan.FromMinutes(1);
            }
        }

        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("BTCUSD", dateRange.Start, dateRange.End, Period.Minute, 20)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("DOGEUSD", dateRange.Start, dateRange.End, Period.Minute, 20)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("ETHUSD", dateRange.Start, dateRange.End, Period.Minute, 20)
        ));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(3));
    }

    [Test]
    public void GetBar_Second_WithThreeSymbols_Each200SecondRange_ShouldSucceed() {
        ISet<string> symbols = new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD"};
        DateTimeOffset start = DateTimeOffset.UtcNow;
        DateRange dateRange = new DateRange(start, start + TimeSpan.FromSeconds(199));
        var result = subject!.GetBars(symbols, dateRange, Period.Second);
        
        foreach (var symbol in symbols) {
            Assert.That(result[symbol], Has.Count.EqualTo(200));
            DateTimeOffset runningStart = start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }

        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("BTCUSD", dateRange.Start, dateRange.End, Period.Second, 200)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("DOGEUSD", dateRange.Start, dateRange.End, Period.Second, 200)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("ETHUSD", dateRange.Start, dateRange.End, Period.Second, 200)
        ));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(3));
    }

    [Test]
    public void GetBars_Second_WithThreeSymbols_Each2000SecondRange_ShouldSucceed() {
        ISet<string> symbols = new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD"};
        DateTimeOffset start = DateTimeOffset.UtcNow;
        DateRange dateRange = new DateRange(start, start + TimeSpan.FromSeconds(1999));
        var result = subject!.GetBars(symbols, dateRange, Period.Second);
        
        foreach (var symbol in symbols) {
            Assert.That(result[symbol], Has.Count.EqualTo(2000));
            DateTimeOffset runningStart = start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }

        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("BTCUSD", dateRange.Start, dateRange.End, Period.Second, 1000)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("BTCUSD", dateRange.Start + TimeSpan.FromSeconds(1000), dateRange.End, Period.Second, 1000)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("DOGEUSD", dateRange.Start, dateRange.End, Period.Second, 1000)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("DOGEUSD", dateRange.Start + TimeSpan.FromSeconds(1000), dateRange.End, Period.Second, 1000)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("ETHUSD", dateRange.Start, dateRange.End, Period.Second, 1000)
        ));
        Assert.That(binanceClient!.GetKlinesCalls, Contains.Item(
            new GetKlinesCall("ETHUSD", dateRange.Start + TimeSpan.FromSeconds(1000), dateRange.End, Period.Second, 1000)
        ));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(6));
    }
    
    [Test]
    public void GetBar_Second_WithThreeSymbols_Each2700SecondRange_ShouldSucceed() {
        ISet<string> symbols = new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD"};
        DateTimeOffset start = DateTimeOffset.UtcNow;
        DateRange dateRange = new DateRange(start, start + TimeSpan.FromSeconds(2699));
        var result = subject!.GetBars(symbols, dateRange, Period.Second);
        
        foreach (var symbol in symbols) {
            Assert.That(result[symbol], Has.Count.EqualTo(2700));
            DateTimeOffset runningStart = start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }

        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("BTCUSD", dateRange.Start, dateRange.End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("BTCUSD", dateRange.Start + TimeSpan.FromSeconds(1000), dateRange.End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("BTCUSD", dateRange.Start + TimeSpan.FromSeconds(2000), dateRange.End, Period.Second, 700))
        );
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("DOGEUSD", dateRange.Start, dateRange.End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("DOGEUSD", dateRange.Start + TimeSpan.FromSeconds(1000), dateRange.End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("DOGEUSD", dateRange.Start + TimeSpan.FromSeconds(2000), dateRange.End, Period.Second, 700))
        );
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("ETHUSD", dateRange.Start, dateRange.End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("ETHUSD", dateRange.Start + TimeSpan.FromSeconds(1000), dateRange.End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("ETHUSD", dateRange.Start + TimeSpan.FromSeconds(2000), dateRange.End, Period.Second, 700))
        );
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(9));
    }

    [Test]
    public void GetBars_Second_WithThreeSymbols_Each200000SecondRange_ShouldSucceed() {
        ISet<string> symbols = new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD", "LINKUSD", "SUSHIUSD"};
        DateTimeOffset start = DateTimeOffset.UtcNow;
        DateRange dateRange = new DateRange(start, start + TimeSpan.FromSeconds(199_999));
        var result = subject!.GetBars(symbols, dateRange, Period.Second);
        
        foreach (var symbol in symbols) {
            Assert.That(result[symbol], Has.Count.EqualTo(200_000));
            DateTimeOffset runningStart = start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }
        
        Assert.That(binanceClient!.GetKlinesCalls, Has.Count.EqualTo(1000));
        Assert.That(binanceClient!.WaitCalls, Has.Count.EqualTo(0));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(1000));
    }

    [Test]
    public void GetBars_Second_WithThreeSymbols_Each200001SecondRange_ShouldSucceed() {
        ISet<string> symbols = new HashSet<string> {"BTCUSD", "ETHUSD", "DOGEUSD", "LINKUSD", "SUSHIUSD"};
        DateTimeOffset start = DateTimeOffset.UtcNow;
        DateRange dateRange = new DateRange(start, start + TimeSpan.FromSeconds(200_000));
        var result = subject!.GetBars(symbols, dateRange, Period.Second);
        
        foreach (var symbol in symbols) {
            Assert.That(result[symbol], Has.Count.EqualTo(200_001));
            DateTimeOffset runningStart = start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }
        
        Assert.That(binanceClient!.GetKlinesCalls, Has.Count.EqualTo(1005));
        Assert.That(binanceClient!.WaitCalls, Has.Count.EqualTo(1));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(5));
    }

    [Test]
    public void GetBars_Second_WithThreeSymbols_VariousRanges_ShouldSucceed() {
        DateTimeOffset start = DateTimeOffset.UtcNow;
        
        Dictionary<string, DateRange> symbols = new Dictionary<string, DateRange> {
            {"BTCUSD", new DateRange(start, start + TimeSpan.FromSeconds(99))},
            {"ETHUSD", new DateRange(start + TimeSpan.FromSeconds(50), start + TimeSpan.FromSeconds(499))},
            {"DOGEUSD", new DateRange(start + TimeSpan.FromSeconds(300), start + TimeSpan.FromSeconds(799))}
        };

        Dictionary<string, int> expectedCounts = new Dictionary<string, int>() {
            { "BTCUSD", 100 },
            { "ETHUSD", 450 },
            { "DOGEUSD", 500 }
        };

        var result = subject!.GetBars(symbols, Period.Second);
        
        foreach (var symbol in symbols.Keys) {
            Assert.That(result[symbol], Has.Count.EqualTo(expectedCounts[symbol]));
            
            DateTimeOffset runningStart = symbols[symbol].Start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }
        
        Assert.That(binanceClient!.WaitCalls, Has.Count.EqualTo(0));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(3));
        
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("BTCUSD", symbols["BTCUSD"].Start, symbols["BTCUSD"].End, Period.Second, 100))
        );
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("ETHUSD", symbols["ETHUSD"].Start, symbols["ETHUSD"].End, Period.Second, 450))
        );
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("DOGEUSD", symbols["DOGEUSD"].Start, symbols["DOGEUSD"].End, Period.Second, 500))
        );
    }

    [Test]
    public void GetBars_Second_WithThreeSymbols_VariousRanges_MultipleCalls_ShouldSucceed() {
        DateTimeOffset start = DateTimeOffset.UtcNow;
        
        Dictionary<string, DateRange> symbols = new Dictionary<string, DateRange> {
            {"BTCUSD", new DateRange(start, start + TimeSpan.FromSeconds(1499))},
            {"ETHUSD", new DateRange(start + TimeSpan.FromSeconds(50), start + TimeSpan.FromSeconds(499))},
            {"DOGEUSD", new DateRange(start + TimeSpan.FromSeconds(300), start + TimeSpan.FromSeconds(799))}
        };

        Dictionary<string, int> expectedCounts = new Dictionary<string, int>() {
            { "BTCUSD", 1500 },
            { "ETHUSD", 450 },
            { "DOGEUSD", 500 }
        };

        var result = subject!.GetBars(symbols, Period.Second);
        
        foreach (var symbol in symbols.Keys) {
            Assert.That(result[symbol], Has.Count.EqualTo(expectedCounts[symbol]));
            
            DateTimeOffset runningStart = symbols[symbol].Start.Truncate(Period.Second.Time);
            foreach (Bar bar in result[symbol]) {
                Assert.That(bar.Symbol, Is.EqualTo(symbol));
                Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
                Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));
                runningStart += TimeSpan.FromSeconds(1);
            }
        }
        
        Assert.That(binanceClient!.WaitCalls, Has.Count.EqualTo(0));
        Assert.That(binanceClient.UsedWeight, Is.EqualTo(4));
        
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("BTCUSD", symbols["BTCUSD"].Start, symbols["BTCUSD"].End, Period.Second, 1000))
                .And.Contains(new GetKlinesCall("BTCUSD", symbols["BTCUSD"].Start + TimeSpan.FromSeconds(1000), symbols["BTCUSD"].End, Period.Second, 500))
        );
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("ETHUSD", symbols["ETHUSD"].Start, symbols["ETHUSD"].End, Period.Second, 450))
        );
        Assert.That(binanceClient!.GetKlinesCalls, 
            Contains.Item(new GetKlinesCall("DOGEUSD", symbols["DOGEUSD"].Start, symbols["DOGEUSD"].End, Period.Second, 500))
        );
    }
}