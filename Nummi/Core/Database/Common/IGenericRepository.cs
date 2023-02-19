using System.Linq.Expressions;

namespace Nummi.Core.Database.Common; 

public interface IGenericRepository<ID, E> where E : class {
    public void Commit();
    
    public E? FindNullableById(ID id);
    public E FindById(ID id);
    public E RequireById(ID id);
    
    public E Add(E entity);
    public void Remove(E entity);
    public IEnumerable<E> FindAll();
    public void LoadProperty<P>(E entity, Expression<Func<E, P?>> propertyExpression) where P : class;
    public void LoadCollection<P>(E entity, Expression<Func<E, IEnumerable<P>>> propertyExpression) where P : class;
}