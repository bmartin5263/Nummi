using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.Common;

namespace NummiTests.Tests.Unit.Domain; 

public class KsuidTest {

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Equals_TwoEqualKsuids_ShouldReturnTrue()
    {
        var id1 = Ksuid.Generate();
        var id2 = Ksuid.FromString(id1.ToString());
        Assert.Multiple(() =>
        {
            Assert.That(id1 == id2, Is.True);
            Assert.That(id1.Equals(id2), Is.True);
            Assert.That(id1.GetHashCode(), Is.EqualTo(id2.GetHashCode()));
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Equals_TwoNonEqualKsuids_ShouldReturnFalse()
    {
        var id1 = Ksuid.Generate();
        var id2 = Ksuid.Generate();
        Assert.Multiple(() => {
            Assert.That(id1 != id2, Is.True);
            Assert.That(id1.Equals(id2), Is.False);
            Assert.That(id1.GetHashCode(), Is.Not.EqualTo(id2.GetHashCode()));
        });
    }

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Comparisons_ThreeKsuids_ShouldCompareByCreationTime() {
        var id1 = Ksuid.Generate();
        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
        var id2 = Ksuid.Generate();
        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
        var id3 = Ksuid.Generate();
        
        Assert.Multiple(() => {
            var self = id1;
            Assert.That(id1.CompareTo(self), Is.EqualTo(0));
            Assert.That(id1 > self, Is.False);
            Assert.That(id1 >= self, Is.True);
            Assert.That(id1 < self, Is.False);
            Assert.That(id1 <= self, Is.True);
            
            Assert.That(id1.CompareTo(id2), Is.LessThan(0));
            Assert.That(id1 > id2, Is.False);
            Assert.That(id1 >= id2, Is.False);
            Assert.That(id1 < id2, Is.True);
            Assert.That(id1 <= id2, Is.True);
            
            Assert.That(id1.CompareTo(id3), Is.LessThan(0));
            Assert.That(id1 > id3, Is.False);
            Assert.That(id1 >= id3, Is.False);
            Assert.That(id1 < id3, Is.True);
            Assert.That(id1 <= id3, Is.True);
            
            self = id2;
            Assert.That(id2.CompareTo(id1), Is.GreaterThan(0));
            Assert.That(id2 > id1, Is.True);
            Assert.That(id2 >= id1, Is.True);
            Assert.That(id2 < id1, Is.False);
            Assert.That(id2 <= id1, Is.False);
            
            Assert.That(id2.CompareTo(self), Is.EqualTo(0));
            Assert.That(id2 > self, Is.False);
            Assert.That(id2 >= self, Is.True);
            Assert.That(id2 < self, Is.False);
            Assert.That(id2 <= self, Is.True);
            
            Assert.That(id2.CompareTo(id3), Is.LessThan(0));
            Assert.That(id2 > id3, Is.False);
            Assert.That(id2 >= id3, Is.False);
            Assert.That(id2 < id3, Is.True);
            Assert.That(id2 <= id3, Is.True);
            
            self = id3;
            Assert.That(id3.CompareTo(id1), Is.GreaterThan(0));
            Assert.That(id3 > id1, Is.True);
            Assert.That(id3 >= id1, Is.True);
            Assert.That(id3 < id1, Is.False);
            Assert.That(id3 <= id1, Is.False);
            
            Assert.That(id3.CompareTo(id2), Is.GreaterThan(0));
            Assert.That(id3 > id2, Is.True);
            Assert.That(id3 >= id2, Is.True);
            Assert.That(id3 < id2, Is.False);
            Assert.That(id3 <= id2, Is.False);
            
            Assert.That(id3.CompareTo(self), Is.EqualTo(0));
            Assert.That(id3 > self, Is.False);
            Assert.That(id3 >= self, Is.True);
            Assert.That(id3 < self, Is.False);
            Assert.That(id3 <= self, Is.True);
        });
    }
}