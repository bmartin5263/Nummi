namespace Nummi.Core.Events; 

public interface EventPublisher {
    public IList<IDomainEvent> DomainEvents { get; }
    public void Raise(IDomainEvent domainEvent) {
        DomainEvents.Add(domainEvent);
    }
}