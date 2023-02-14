using System.Globalization;

namespace Nummi.Core.Domain.Common; 

public readonly record struct Dollars(decimal amount) {
    public override string ToString() => amount.ToString(CultureInfo.InvariantCulture);
}