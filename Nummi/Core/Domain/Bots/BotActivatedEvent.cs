using Nummi.Core.Events;

namespace Nummi.Core.Domain.Bots; 

public record BotActivatedEvent(
    BotId BotId
) : IDomainEvent;