namespace Nummi.Core.Domain.Common; 

// https://dotnetcoretutorials.com/2022/03/16/auto-updating-created-updated-and-deleted-timestamps-in-entity-framework/
public interface EventPublisher {
    public IList<IDomainEvent> DomainEvents { get; }
}