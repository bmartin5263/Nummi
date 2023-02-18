using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.Common;

namespace NummiTests.Unit.Domain; 

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
            Assert.That(id1.CompareTo(id1), Is.EqualTo(0));
            Assert.That(id1 > id1, Is.False);
            Assert.That(id1 >= id1, Is.True);
            Assert.That(id1 < id1, Is.False);
            Assert.That(id1 <= id1, Is.True);
            
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
            
            Assert.That(id2.CompareTo(id1), Is.GreaterThan(0));
            Assert.That(id2 > id1, Is.True);
            Assert.That(id2 >= id1, Is.True);
            Assert.That(id2 < id1, Is.False);
            Assert.That(id2 <= id1, Is.False);
            
            Assert.That(id2.CompareTo(id2), Is.EqualTo(0));
            Assert.That(id2 > id2, Is.False);
            Assert.That(id2 >= id2, Is.True);
            Assert.That(id2 < id2, Is.False);
            Assert.That(id2 <= id2, Is.True);
            
            Assert.That(id2.CompareTo(id3), Is.LessThan(0));
            Assert.That(id2 > id3, Is.False);
            Assert.That(id2 >= id3, Is.False);
            Assert.That(id2 < id3, Is.True);
            Assert.That(id2 <= id3, Is.True);
            
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
            
            Assert.That(id3.CompareTo(id3), Is.EqualTo(0));
            Assert.That(id3 > id3, Is.False);
            Assert.That(id3 >= id3, Is.True);
            Assert.That(id3 < id3, Is.False);
            Assert.That(id3 <= id3, Is.True);
        });
    }
}