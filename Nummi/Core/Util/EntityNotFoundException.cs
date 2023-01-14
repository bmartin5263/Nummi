namespace Nummi.Core.Util; 

public class EntityNotFoundException<T> : Exception {
    private object Id { get; }

    public EntityNotFoundException(object id) {
        Id = id;
    }
}