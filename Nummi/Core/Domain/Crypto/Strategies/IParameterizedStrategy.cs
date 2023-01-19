namespace Nummi.Core.Domain.Crypto.Strategies; 

public interface IParameterizedStrategy {
    public Type ParameterObjectType { get; }
    public object Parameters { get; }
    public void AcceptParameters(object parameters);
}

public interface IParameterizedStrategy<T> : IParameterizedStrategy {
    public new T Parameters { get; }
    public void AcceptParameters(T parameters);

    object IParameterizedStrategy.Parameters => Parameters!;

    void IParameterizedStrategy.AcceptParameters(object parameters) {
        AcceptParameters((T)parameters);
    }
}