using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public abstract class GenericRepository<ID, E> : IGenericRepository<ID, E> where E : class where ID : notnull {
    protected ITransaction Transaction { get; }
    protected EFCoreContext Context => (Transaction.DbContext as EFCoreContext)!;

    protected GenericRepository(ITransaction transaction) {
        Transaction = transaction;
    }

    public virtual E Add(E entity) {
        return Context.Set<E>().Add(entity).Entity;
    }

    public virtual void AddRange(IEnumerable<E> entity) {
        Context.Set<E>().AddRange(entity);
    }
    
    public virtual Task AddRangeAsync(IEnumerable<E> entity) { 
        return Context.Set<E>().AddRangeAsync(entity);
    }

    public virtual long AddRangeIfNotExists(IEnumerable<E> entity) {
        throw new NotImplementedException();
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

    public void Commit() {
        Context.SaveChanges();
    }

    public virtual bool ExistsById(ID id) {
        return FindNullableById(id) != null;
    }

    public virtual E? FindNullableById(ID id) {
        var entity = Context.Set<E>().Find(id);
        if (entity == null) {
            return null;
        }
        
        if (!typeof(Audited).IsAssignableFrom(typeof(E))) {
            return entity;
        }
        
        var audited = (entity as Audited)!;
        return audited.IsDeleted ? null : entity;
    }
    
    public virtual E FindById(ID id) {
        var entity = Context.Set<E>().Find(id)
            .OrElseThrow(() => EntityNotFoundException<E>.IdNotFound(id));

        if (!typeof(Audited).IsAssignableFrom(typeof(E))) {
            return entity;
        }
        
        var audited = (entity as Audited)!;
        if (audited.IsDeleted) {
            throw EntityNotFoundException<E>.IdNotFound(id);
        }

        return entity;
    }
    
    public virtual E RequireById(ID id) {
        return Context.Set<E>().Find(id)
            .OrElseThrow(() => new EntityMissingException<E>(id));
    }

    public virtual IEnumerable<E> FindAll() {
        return Context.Set<E>()
            .AsEnumerable()
            .Where(v => {
                if (!typeof(Audited).IsAssignableFrom(typeof(E))) {
                    return true;
                }

                var audited = (v as Audited)!;
                return !audited.IsDeleted;
            })
            .ToList();
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