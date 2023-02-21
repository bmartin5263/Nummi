using System.Linq.Expressions;
using System.Reflection;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.New.User;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace NummiTests.Utils; 

public class GenericTestRepository<ID, T> : IGenericRepository<ID, T> where T : class where ID : notnull {

    protected IDictionary<ID, T> Table { get; } = new Dictionary<ID, T>();

    public T Add(T entity) {
        var id = GetId(entity);
        Table[id] = entity;
        return entity;
    }

    public void Remove(T entity) {
        var id = GetId(entity);
        Table.Remove(id);
    }

    public void Commit() {
        
    }

    public bool ExistsById(ID id) {
        return FindNullableById(id) != null;
    }

    public T? FindNullableById(ID id) {
        return Table.TryGetValue(id, out T? value) ? value : null;
    }

    public T FindById(ID id) {
        return Table[id].OrElseThrow(() => EntityNotFoundException<T>.IdNotFound(id));
    }

    public T RequireById(ID id) {
        return Table[id].OrElseThrow(() => new EntityMissingException<T>(id));
    }

    public IEnumerable<T> FindAll() {
        return Table.Values;
    }

    public void LoadProperty<P>(T entity, Expression<Func<T, P?>> propertyExpression) where P : class {
        
    }

    public void LoadCollection<P>(T entity, Expression<Func<T, IEnumerable<P>>> propertyExpression) where P : class {
        
    }

    public ID GetId(T entity) {
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

public class BarTestRepository : IBarRepository {
    public Bar? FindById(string symbol, DateTimeOffset openTime, TimeSpan period) {
        throw new NotImplementedException();
    }

    public List<Bar> FindByIdRange(string symbol, DateTimeOffset startOpenTime, DateTimeOffset endOpenTime, TimeSpan period) {
        throw new NotImplementedException();
    }

    public void Add(Bar bar) {
        throw new NotImplementedException();
    }

    public int AddRange(IEnumerable<Bar> bars) {
        throw new NotImplementedException();
    }

    public void Save() {
        throw new NotImplementedException();
    }
}

public class BotTestRepository : GenericTestRepository<Ksuid, Bot>, IBotRepository {
    
}

public class SimulationTestRepository : GenericTestRepository<Ksuid, Simulation>, ISimulationRepository {
    
}

public class StrategyTestRepository : GenericTestRepository<Ksuid, Strategy>, IStrategyRepository {
    
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