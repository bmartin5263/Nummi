using System.Linq.Expressions;

namespace Nummi.Core.Database.Common; 

public interface IGenericRepository<ID, E> where E : class {
    public E Add(E entity);
    public void Remove(E entity);
    public E? FindById(ID id);
    public IEnumerable<E> FindAll();
    public void LoadProperty<P>(E entity, Expression<Func<E, P?>> propertyExpression) where P : class;
    public void LoadCollection<P>(E entity, Expression<Func<E, IEnumerable<P>>> propertyExpression) where P : class;
}