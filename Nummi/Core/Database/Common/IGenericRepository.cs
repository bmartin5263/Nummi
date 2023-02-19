using System.Linq.Expressions;

namespace Nummi.Core.Database.Common; 

public interface IGenericRepository<ID, E> where E : class {
    public void Commit();
    
    public bool ExistsById(ID id);
    
    /**
     * Returns the entity with the given id, or null if none can be found
     */
    public E? FindNullableById(ID id);
    
    /**
     * Returns the entity with the given id, or throws a User-type exception if none can be found. This method
     * should be preferred over `RequireById` in situations where the input `id` is user-supplied and might
     * be invalid
     */
    public E FindById(ID id);
    
    /**
     * Returns the entity with the given id, or throws a System-type exception if none can be found. This method
     * should be preferred over `FindById` in situations where the input `id` is system-supplied and is expected
     * never to be invalid unless some other process went wrong
     */
    public E RequireById(ID id);
    
    public E Add(E entity);
    public void Remove(E entity);
    public IEnumerable<E> FindAll();
    public void LoadProperty<P>(E entity, Expression<Func<E, P?>> propertyExpression) where P : class;
    public void LoadCollection<P>(E entity, Expression<Func<E, IEnumerable<P>>> propertyExpression) where P : class;
}