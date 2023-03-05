using System.Net;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Exceptions;
using Nummi.Core.External.Binance;
using Nummi.Core.Util;

namespace IntegrationTests.Tests.Client; 

public class BinanceClientTest {

    private readonly BinanceClient subject = new();

    [Test]
    public void GetKlines_MinutePeriod_SameStartAndEnd_ShouldReturn1Kline() {
        DateTimeOffset time = DateTimeOffset.UtcNow.Truncate(TimeSpan.FromMinutes(1)) - TimeSpan.FromMinutes(10);
        
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
        Assert.That(response.Content[0].OpenTime, Is.EqualTo(time));
        Assert.That(response.Content[0].Period, Is.EqualTo(Period.Minute.Time));
    }

    [Test]
    public void GetKlines_SecondPeriod_SameStartAndEnd_ShouldReturn1Kline() {
        DateTimeOffset time = DateTimeOffset.UtcNow.Truncate(TimeSpan.FromSeconds(1)) - TimeSpan.FromSeconds(10);
        
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
        Assert.That(response.Content[0].OpenTime, Is.EqualTo(time));
        Assert.That(response.Content[0].Period, Is.EqualTo(Period.Second.Time));
    }

    [Test]
    public void GetKlines_MinutePeriod_End5MinutesFromStart_ShouldReturn6Klines() {
        DateTimeOffset start = DateTimeOffset.UtcNow.Truncate(TimeSpan.FromMinutes(1)) - TimeSpan.FromMinutes(10);
        DateTimeOffset end = start + TimeSpan.FromMinutes(5);
        
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

        DateTimeOffset runningStart = start;
        foreach (Bar bar in response.Content) {
            Assert.That(bar.Symbol, Is.EqualTo("BTCUSD"));
            Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
            Assert.That(bar.Period, Is.EqualTo(Period.Minute.Time));

            runningStart += TimeSpan.FromMinutes(1);
        }
    }

    [Test]
    public void GetKlines_SecondPeriod_End999SecondsFromStart_ShouldReturn1000Klines() {
        DateTimeOffset start = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(100);
        DateTimeOffset end = start + TimeSpan.FromSeconds(999);
        
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

        DateTimeOffset runningStart = start.Truncate(Period.Second.Time);
        foreach (Bar bar in response.Content) {
            Assert.That(bar.Symbol, Is.EqualTo("BTCUSD"));
            Assert.That(bar.OpenTime, Is.EqualTo(runningStart));
            Assert.That(bar.Period, Is.EqualTo(Period.Second.Time));

            runningStart += TimeSpan.FromSeconds(1);
        }
    }
    
    [Test]
    public void GetKlines_SecondPeriod_OverUnderLimit_ShouldThrow() {
        Assert.Throws<InvalidUserArgumentException>(() => {
            subject.GetKlines(
                symbol: "BTCUSD",
                startTime: DateTimeOffset.UtcNow,
                endTime: DateTimeOffset.UtcNow,
                period: Period.Second,
                limit: 1001 // over limit!
            );
        });
        
        Assert.Throws<InvalidUserArgumentException>(() => {
            subject.GetKlines(
                symbol: "BTCUSD",
                startTime: DateTimeOffset.UtcNow,
                endTime: DateTimeOffset.UtcNow,
                period: Period.Second,
                limit: -1 // under limit!
            );
        });
    }
}