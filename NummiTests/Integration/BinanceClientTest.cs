using System.Net;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace NummiTests.Integration; 

public class BinanceClientTest {

    private readonly BinanceClient subject = new();

    [Test]
    public void GetKlines_MinutePeriod_SameStartAndEnd_ShouldReturn1Kline() {
        DateTime time = DateTime.UtcNow.Truncate(TimeSpan.FromMinutes(1)) - TimeSpan.FromMinutes(10);
        
        BinanceResponse<IList<Bar>> response = subject.GetKlines(
            symbol: "BTCUSD", 
            startTime: time, 
            endTime: time, 
            period: Period.Minute, 
            limit: 100
        );
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.RetryAfter, Is.Null);
        Assert.That(response.UsedWeight1M, Is.GreaterThan(0));
        Assert.That(response.Content, Has.Count.EqualTo(1));
        Assert.That(response.Content[0].Symbol, Is.EqualTo("BTCUSD"));
        Assert.That(response.Content[0].OpenTimeUtc, Is.EqualTo(time));
        Assert.That(response.Content[0].OpenTimeUnixMs, Is.EqualTo(time.ToUnixTimeMs()));
        Assert.That(response.Content[0].PeriodMs, Is.EqualTo(Period.Minute.UnixMs));
    }

    [Test]
    public void GetKlines_SecondPeriod_SameStartAndEnd_ShouldReturn1Kline() {
        DateTime time = DateTime.UtcNow.Truncate(TimeSpan.FromSeconds(1)) - TimeSpan.FromSeconds(10);
        
        BinanceResponse<IList<Bar>> response = subject.GetKlines(
            symbol: "BTCUSD", 
            startTime: time, 
            endTime: time, 
            period: Period.Second, 
            limit: 100
        );
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.RetryAfter, Is.Null);
        Assert.That(response.UsedWeight1M, Is.GreaterThan(0));
        Assert.That(response.Content, Has.Count.EqualTo(1));
        Assert.That(response.Content[0].Symbol, Is.EqualTo("BTCUSD"));
        Assert.That(response.Content[0].OpenTimeUtc, Is.EqualTo(time));
        Assert.That(response.Content[0].OpenTimeUnixMs, Is.EqualTo(time.ToUnixTimeMs()));
        Assert.That(response.Content[0].PeriodMs, Is.EqualTo(Period.Second.UnixMs));
    }

    [Test]
    public void GetKlines_MinutePeriod_End5MinutesFromStart_ShouldReturn6Klines() {
        DateTime start = DateTime.UtcNow.Truncate(TimeSpan.FromMinutes(1)) - TimeSpan.FromMinutes(10);
        DateTime end = start + TimeSpan.FromMinutes(5);
        
        BinanceResponse<IList<Bar>> response = subject.GetKlines(
            symbol: "BTCUSD", 
            startTime: start, 
            endTime: end, 
            period: Period.Minute, 
            limit: 100
        );
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.RetryAfter, Is.Null);
        Assert.That(response.UsedWeight1M, Is.GreaterThan(0));
        Assert.That(response.Content, Has.Count.EqualTo(6));

        DateTime runningStart = start;
        foreach (Bar bar in response.Content) {
            Assert.That(bar.Symbol, Is.EqualTo("BTCUSD"));
            Assert.That(bar.OpenTimeUtc, Is.EqualTo(runningStart));
            Assert.That(bar.OpenTimeUnixMs, Is.EqualTo(runningStart.ToUnixTimeMs()));
            Assert.That(bar.PeriodMs, Is.EqualTo(Period.Minute.UnixMs));

            runningStart += TimeSpan.FromMinutes(1);
        }
    }

    [Test]
    public void GetKlines_SecondPeriod_End999SecondsFromStart_ShouldReturn1000Klines() {
        DateTime start = DateTime.UtcNow - TimeSpan.FromMinutes(100);
        DateTime end = start + TimeSpan.FromSeconds(999);
        
        BinanceResponse<IList<Bar>> response = subject.GetKlines(
            symbol: "BTCUSD", 
            startTime: start, 
            endTime: end, 
            period: Period.Second, 
            limit: 1000
        );
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.RetryAfter, Is.Null);
        Assert.That(response.UsedWeight1M, Is.GreaterThan(0));
        Assert.That(response.Content, Has.Count.EqualTo(1000));

        DateTime runningStart = start.Truncate(Period.Second.Time);
        foreach (Bar bar in response.Content) {
            Assert.That(bar.Symbol, Is.EqualTo("BTCUSD"));
            Assert.That(bar.OpenTimeUtc, Is.EqualTo(runningStart));
            Assert.That(bar.OpenTimeUnixMs, Is.EqualTo(runningStart.ToUnixTimeMs()));
            Assert.That(bar.PeriodMs, Is.EqualTo(Period.Second.UnixMs));

            runningStart += TimeSpan.FromSeconds(1);
        }
    }
    
    [Test]
    public void GetKlines_SecondPeriod_OverUnderLimit_ShouldThrow() {
        Assert.Throws<InvalidArgumentException>(() => {
            subject.GetKlines(
                symbol: "BTCUSD",
                startTime: DateTime.UtcNow,
                endTime: DateTime.UtcNow,
                period: Period.Second,
                limit: 1001 // over limit!
            );
        });
        
        Assert.Throws<InvalidArgumentException>(() => {
            subject.GetKlines(
                symbol: "BTCUSD",
                startTime: DateTime.UtcNow,
                endTime: DateTime.UtcNow,
                period: Period.Second,
                limit: -1 // under limit!
            );
        });
    }
}