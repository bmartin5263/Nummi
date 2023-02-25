using System.Linq.Expressions;
using System.Reflection;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.User;
using Nummi.Core.Domain.Simulations;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace NummiTests.Utils; 

public class GenericTestRepository<ID, T> : IGenericRepository<ID, T> where T : class where ID : notnull {

    public IDictionary<ID, T> Table { get; } = new Dictionary<ID, T>();

    public virtual T Add(T entity) {
        var id = GetId(entity);
        if (Table.ContainsKey(id)) {
            throw new InvalidUserArgumentException($"Entity with id already exists {id}");
        }
        Table[id] = entity;
        return entity;
    }

    public virtual void AddRange(IEnumerable<T> entities) {
        foreach (var entity in entities) {
            Add(entity);
        }
    }

    public virtual long AddRangeIfNotExists(IEnumerable<T> entities) {
        var added = 0;
        foreach (var entity in entities) {
            var id = GetId(entity);
            if (!Table.ContainsKey(id)) {
                Table[id] = entity;
                ++added;
            }
        }

        return added;
    }

    public virtual Task AddRangeAsync(IEnumerable<T> entities) {
        AddRange(entities);
        return Task.CompletedTask;
    }

    public virtual void Remove(T entity) {
        var id = GetId(entity);
        Table.Remove(id);
    }

    public void Commit() {
        
    }

    public virtual bool ExistsById(ID id) {
        return FindNullableById(id) != null;
    }

    public virtual T? FindNullableById(ID id) {
        return Table.TryGetValue(id, out T? value) ? value : null;
    }

    public virtual T FindById(ID id) {
        return Table[id].OrElseThrow(() => EntityNotFoundException<T>.IdNotFound(id));
    }

    public virtual T RequireById(ID id) {
        return Table[id].OrElseThrow(() => new EntityMissingException<T>(id));
    }

    public virtual IEnumerable<T> FindAll() {
        return Table.Values;
    }

    public virtual void LoadProperty<P>(T entity, Expression<Func<T, P?>> propertyExpression) where P : class {
        
    }

    public virtual void LoadCollection<P>(T entity, Expression<Func<T, IEnumerable<P>>> propertyExpression) where P : class {
        
    }

    public virtual ID GetId(T entity) {
        var type = entity.GetType();
        var properties = type.GetProperties();
        PropertyInfo? idProperty = null;
        var className = typeof(T).Name;
        foreach (var propertyInfo in properties) {
            if ((propertyInfo.Name == "Id" || propertyInfo.Name == $"{className}Id") && propertyInfo.PropertyType == typeof(ID)) {
                if (idProperty == null) {
                    idProperty = propertyInfo;
                }
                else {
                    throw new Exception($"Found multiple Id properties: {propertyInfo.Name}, {idProperty.Name}");
                }
            }
        }

        if (idProperty == null) {
            throw new Exception("Did not find Id property");
        }
        
        return (ID) idProperty.GetValue(entity)!;
    }
}

public class BotTestRepository : GenericTestRepository<Ksuid, Bot>, IBotRepository {
    
}

public class SimulationTestRepository : GenericTestRepository<Ksuid, Simulation>, ISimulationRepository {
    
}

public class StrategyTestRepository : GenericTestRepository<Ksuid, Strategy>, IStrategyRepository {
    
}


public class BarTestRepository : GenericTestRepository<BarId, Bar>, IBarRepository {
    public List<Bar> FindByIdRange(string symbol, DateTimeOffset startOpenTime, DateTimeOffset endOpenTime, TimeSpan period) {
        return Table
            .Where(e =>
                e.Key.Symbol == symbol
                && e.Key.Period == period
                && e.Key.OpenTime >= startOpenTime
                && e.Key.OpenTime <= endOpenTime
            )
            .Select(e => e.Value)
            .ToList();
    }
    
    public void Add(IDictionary<string, List<Bar>> barDict) {
        foreach (List<Bar> barList in barDict.Values) {
            foreach (Bar bar in barList) {
                var id = bar.Id;
                if (Table.ContainsKey(id)) {
                    throw new InvalidUserArgumentException($"Bar already exists {bar}");
                }
                Table[id] = bar;
            }
        }
    }

    public override long AddRangeIfNotExists(IEnumerable<Bar> entities) {
        var added = 0;
        foreach (var bar in entities) {
            var id = bar.Id;
            if (!Table.ContainsKey(id)) {
                Table[id] = bar;
                ++added;
            }
        }

        return added;
    }
}

public class StrategyTemplateTestRepository : GenericTestRepository<Ksuid, StrategyTemplate>, IStrategyTemplateRepository {
    public void RemoveAllByUserId(Ksuid userId) {
        var removeList = new List<Ksuid>();
        foreach (KeyValuePair<Ksuid,StrategyTemplate> pair in Table) {
            if (pair.Value.UserId == userId) {
                removeList.Add(pair.Key);
            }
        }

        foreach (var id in removeList) {
            Table.Remove(id);
        }
    }
}

public class TestUserRepository : GenericTestRepository<Ksuid, NummiUser>, IUserRepository {
    
}

public class TestTransaction : ITransaction {
    public required IBarRepository BarRepository { get; init; }
    public required IBotRepository BotRepository { get; init; }
    public required ISimulationRepository SimulationRepository { get; init; }
    public required IStrategyRepository StrategyRepository  { get; init; }
    public required IStrategyTemplateRepository StrategyTemplateRepository { get; init; }
    public required IUserRepository UserRepository { get; init; }

    public void Commit() {
        
    }

    public object DbContext { get; } = new object();

    public void SaveAndDispose() {
        
    }
    
    public void Dispose() {
        
    }
}