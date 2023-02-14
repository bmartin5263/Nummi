namespace Nummi.Core.Domain.Crypto.Strategies; 

public interface IParameterizedStrategy {
    public Type ParameterObjectType { get; }
    public object Parameters { get; set; }
}

public interface IParameterizedStrategy<T> : IParameterizedStrategy {
    public new T Parameters { get; set; }
    
    object IParameterizedStrategy.Parameters {
        get => Parameters!;
        set => Parameters = (T) value;
    }
}