namespace Nummi.Core.Domain.Common; 

public interface EventPublisher {
    public IList<IDomainEvent> DomainEvents { get; }
}