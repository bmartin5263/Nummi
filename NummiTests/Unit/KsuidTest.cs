using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.Common;

namespace NummiTests.Unit; 

public class KsuidTest {

    [Test]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Ksuid_ShouldHaveEqualityComparisons() {
        var id1 = Ksuid.Generate();
        var id2 = Ksuid.FromString(id1.ToString());

        Assert.True(id1 == id2);
        Assert.True(id1.Equals(id2));
        Assert.That(id1.GetHashCode(), Is.EqualTo(id2.GetHashCode()));
    }
    
}