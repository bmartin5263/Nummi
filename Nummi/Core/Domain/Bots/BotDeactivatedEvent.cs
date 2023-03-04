using Nummi.Core.Events;

namespace Nummi.Core.Domain.Bots; 

public record BotDeactivatedEvent(
    BotId BotId
) : IDomainEvent;