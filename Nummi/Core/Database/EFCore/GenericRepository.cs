using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;

namespace Nummi.Core.Database.EFCore; 

public abstract class GenericRepository<ID, E> : IGenericRepository<ID, E> where E : class {
    protected EFCoreContext Context { get; }

    protected GenericRepository(EFCoreContext context) {
        Context = context;
    }

    public virtual void Add(E entity) {
        Context.Set<E>().Add(entity);
    }
    
    public virtual void Remove(E entity) {
        var dbSet = Context.Set<E>();
        if (typeof(Audited).IsAssignableFrom(typeof(E))) {
            (entity as Audited)!.DeletedAt = DateTimeOffset.UtcNow;
            dbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
        else {
            dbSet.Remove(entity);
        }
    }

    public virtual E? FindById(ID id) {
        return Context.Set<E>().Find(id);
    }

    public virtual IEnumerable<E> FindAll() {
        return Context.Set<E>().ToList();
    }

    public virtual void LoadProperty<P>(E entity, Expression<Func<E, P?>> propertyExpression) where P : class {
        Context.Entry(entity)
            .Reference(propertyExpression)
            .Load();
    }

    public virtual void LoadCollection<P>(E entity, Expression<Func<E, IEnumerable<P>>> propertyExpression) where P : class {
        Context.Entry(entity)
            .Collection(propertyExpression)
            .Load();
    }
}