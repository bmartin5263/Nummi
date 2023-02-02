namespace Nummi.Core.Exceptions; 

public class EntityNotFoundException<T> : UserException {
    public EntityNotFoundException(object id): 
        base($"{typeof(T).Name} not found by id: {id}") 
    { }
    
}