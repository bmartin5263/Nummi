namespace Nummi.Core.App.Trading; 

public interface IFundSource {
    public decimal RemainingFunds { get; }
    public void SubtractFunds(decimal amount);
}