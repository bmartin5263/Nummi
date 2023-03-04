using Nummi.Core.Database.EFCore;

namespace Nummi.Core.Events;

public class EventDispatcher {

    private Dictionary<Type, List<Action<object>>> RegistrationMap { get; } = new();

    public void Dispatch(IEnumerable<IDomainEvent> events) {
        foreach (var domainEvent in events) {
            if (!RegistrationMap.TryGetValue(domainEvent.GetType(), out var actions)) {
                continue;
            }
            
            foreach (var del in actions) {
                del(domainEvent);
            }
        }
    }

    public void OnEvent<T>(Action<T> action) where T : IDomainEvent {
        // IList<Delegate>? actions;
        // if (!RegistrationMap.TryGetValue(typeof(T), out actions)) {
        //     actions = new List<Delegate>();
        //     RegistrationMap[typeof(T)] = actions;
        // }
        void Proxy(object e) => action((T)e);
        var actions = RegistrationMap.GetOrInsert(typeof(T), () => new List<Action<object>>());
        actions.Add(Proxy);
    }
    
}