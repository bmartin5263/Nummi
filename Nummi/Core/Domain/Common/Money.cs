namespace Nummi.Core.Domain.Common; 

public readonly record struct Money(Dollars amount, Currency currency = Currency.USD) {
    public override string ToString() => $"{amount} {currency}";
}