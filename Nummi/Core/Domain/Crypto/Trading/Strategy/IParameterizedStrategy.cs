namespace Nummi.Core.Domain.Crypto.Trading.Strategy; 

public interface IParameterizedStrategy<in T> {
    public Type ParameterObjectType { get; }
    public void AcceptParameters(T parameters);
}