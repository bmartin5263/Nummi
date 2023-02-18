using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.New;

namespace NummiTests.Unit.Domain; 

public class BarTest {

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Equality_TwoEqualBars_ShouldBeTrue()
    {
        var now = DateTimeOffset.UtcNow;
        var bar1 = new Bar(
            symbol: "BTC",
            openTime: now,
            period: TimeSpan.FromMinutes(1),
            open: 10.0m,
            high: 12.0m,
            low: 4.0m,
            close: 11.0m,
            volume: 0.0m
        );
        
        var bar2 = new Bar(
            symbol: "BTC",
            openTime: now,
            period: TimeSpan.FromMinutes(1),
            open: 10.0m,
            high: 12.0m,
            low: 4.0m,
            close: 11.0m,
            volume: 0.0m
        );
        Assert.Multiple(() =>
        {
            Assert.That(bar1.Equals(bar2), Is.True);
            Assert.That(bar1.GetHashCode(), Is.EqualTo(bar2.GetHashCode()));
            Assert.That(bar1 == bar2, Is.True);
            Assert.That(bar1.CompareTo(bar2), Is.Zero);
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Equality_TwoNonEqualBars_ShouldBeFalse()
    {
        var now = DateTimeOffset.UtcNow;
        var bar1 = new Bar(
            symbol: "BTC",
            openTime: now + TimeSpan.FromMinutes(12),
            period: TimeSpan.FromMinutes(1),
            open: 11.0m,
            high: 13.0m,
            low: 5.0m,
            close: 12.0m,
            volume: 0.0m
        );
        
        var bar2 = new Bar(
            symbol: "BTC",
            openTime: now,
            period: TimeSpan.FromMinutes(1),
            open: 10.0m,
            high: 12.0m,
            low: 4.0m,
            close: 11.0m,
            volume: 0.0m
        );
        Assert.Multiple(() =>
        {
            Assert.That(bar1.Equals(bar2), Is.False);
            Assert.That(bar1.GetHashCode(), Is.Not.EqualTo(bar2.GetHashCode()));
            Assert.That(bar1 == bar2, Is.False);
            Assert.That(bar1.CompareTo(bar2), Is.GreaterThan(0));
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Comparisons_ThreeBars_ShouldCompareByOpenTime()
    {
        var openTime = DateTimeOffset.UtcNow;
        var bar1 = CreateBar(openTime);
        var bar2 = CreateBar(openTime + TimeSpan.FromMinutes(1));
        var bar3 = CreateBar(openTime + TimeSpan.FromMinutes(2));
        
        Assert.Multiple(() =>
        {
            Assert.That(bar1.CompareTo(bar1), Is.EqualTo(0));
            Assert.That(bar1.CompareTo(bar2), Is.LessThan(0));
            Assert.That(bar1.CompareTo(bar3), Is.LessThan(0));
            
            Assert.That(bar2.CompareTo(bar1), Is.GreaterThan(0));
            Assert.That(bar2.CompareTo(bar2), Is.EqualTo(0));
            Assert.That(bar2.CompareTo(bar3), Is.LessThan(0));
            
            Assert.That(bar3.CompareTo(bar1), Is.GreaterThan(0));
            Assert.That(bar3.CompareTo(bar2), Is.GreaterThan(0));
            Assert.That(bar3.CompareTo(bar3), Is.EqualTo(0));
        });
    }

    private static Bar CreateBar(DateTimeOffset openTime) {
        return new Bar(
            symbol: "BTC",
            openTime: openTime,
            period: TimeSpan.FromMinutes(1),
            open: 10.0m,
            high: 12.0m,
            low: 4.0m,
            close: 11.0m,
            volume: 0.0m
        );
    }
}