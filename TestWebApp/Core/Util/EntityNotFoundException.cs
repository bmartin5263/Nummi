namespace TestWebApp.Core.Util; 

public class EntityNotFoundException<T> : Exception {
    private Type Type { get; }
    private T Id { get; }

    public EntityNotFoundException(Type type, T id) {
        Type = type;
        Id = id;
    }
}