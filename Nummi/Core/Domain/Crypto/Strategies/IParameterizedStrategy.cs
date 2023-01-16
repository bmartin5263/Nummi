namespace Nummi.Core.Domain.Crypto.Strategies; 

public interface IParameterizedStrategy {
    public Type ParameterObjectType { get; }
    public void AcceptParameters(object parameters);
}

public interface IParameterizedStrategy<in T> : IParameterizedStrategy {
    public void AcceptParameters(T parameters);
    void IParameterizedStrategy.AcceptParameters(object parameters) {
        AcceptParameters((T)parameters);
    }
}