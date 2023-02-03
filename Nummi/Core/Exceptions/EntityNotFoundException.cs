namespace Nummi.Core.Exceptions; 

public class EntityNotFoundException<T> : UserException {
    public EntityNotFoundException(string msg): 
        base(msg) 
    { }

    public static EntityNotFoundException<T> IdNotFound(object id) {
        return new EntityNotFoundException<T>($"{typeof(T).Name} not found by id: {id}");
    }
}