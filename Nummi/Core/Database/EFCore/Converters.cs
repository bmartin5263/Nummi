using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Crypto;
using Nummi.Core.Domain.Simulations;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.User;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore;

[UsedImplicitly]
public class KsuidConverter : ValueConverter<Ksuid, string> {
    public KsuidConverter()
        : base(
            v => v.ToString(),
            v => Ksuid.FromString(v)
        )
    {
    }
}

[UsedImplicitly]
public class NummiJsonConverter<T> : ValueConverter<T, string> {
    public NummiJsonConverter()
        : base(
            v => Serializer.ToJson(v),
            v => Serializer.FromJson<T>(v)!
        )
    {
    }
}

[UsedImplicitly]
public class StrategyFrequencyConverter : ValueConverter<StrategyFrequency, TimeSpan> {
    public StrategyFrequencyConverter()
        : base(
            v => v.AsTimeSpan,
            v => StrategyFrequency.FromTimeSpan(v)
        )
    {
    }
}

[UsedImplicitly]
public class BotIdConverter : ValueConverter<BotId, Guid> {
    public BotIdConverter()
        : base(
            v => v.Value,
            v => BotId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class StrategyIdConverter : ValueConverter<StrategyId, Guid> {
    public StrategyIdConverter()
        : base(
            v => v.Value,
            v => StrategyId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class SimulationIdConverter : ValueConverter<SimulationId, Guid> {
    public SimulationIdConverter()
        : base(
            v => v.Value,
            v => SimulationId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class StrategyTemplateIdConverter : ValueConverter<StrategyTemplateId, Guid> {
    public StrategyTemplateIdConverter()
        : base(
            v => v.Value,
            v => StrategyTemplateId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class IdentityIdConverter : ValueConverter<IdentityId, Guid> {
    public IdentityIdConverter()
        : base(
            v => v.Value,
            v => IdentityId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class StrategyTemplateVersionIdConverter : ValueConverter<StrategyTemplateVersionId, Guid> {
    public StrategyTemplateVersionIdConverter()
        : base(
            v => v.Value,
            v => StrategyTemplateVersionId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class BotLogIdConverter : ValueConverter<BotLogId, Guid> {
    public BotLogIdConverter()
        : base(
            v => v.Value,
            v => BotLogId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class OrderLogIdConverter : ValueConverter<OrderLogId, Guid> {
    public OrderLogIdConverter()
        : base(
            v => v.Value,
            v => OrderLogId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class BotActivationIdConverter : ValueConverter<BotActivationId, Guid> {
    public BotActivationIdConverter()
        : base(
            v => v.Value,
            v => BotActivationId.FromGuid(v)
        )
    {
    }
}

[UsedImplicitly]
public class StrategyLogIdConverter : ValueConverter<StrategyLogId, Guid> {
    public StrategyLogIdConverter()
        : base(
            v => v.Value,
            v => StrategyLogId.FromGuid(v)
        )
    {
    }
}