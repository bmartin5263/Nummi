namespace Nummi.Core.Domain.Crypto.Strategies; 

public interface IStatefulStrategy {
    public Type StateObjectType { get; }
    public object State { get; set; }
}

public interface IStatefulStrategy<T> : IStatefulStrategy {
    public new T State { get; set; }
    
    object IStatefulStrategy.State {
        get => State!;
        set => State = (T) value;
    }
}