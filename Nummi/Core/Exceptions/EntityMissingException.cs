namespace Nummi.Core.Exceptions; 

public class EntityMissingException<T> : SystemException {
    public EntityMissingException(object id): 
        base($"{typeof(T)} not found by id: {id}") 
    { }
}