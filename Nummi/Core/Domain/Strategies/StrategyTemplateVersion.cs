using System.Text.Json.Serialization;
using Nummi.Core.Domain.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

public readonly record struct StrategyTemplateVersionId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static StrategyTemplateVersionId Generate() => new(Guid.NewGuid());
    public static StrategyTemplateVersionId FromGuid(Guid id) => new(id);
    public static StrategyTemplateVersionId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

[JsonConverter(typeof(Serializer.AbstractTypeConverter<StrategyTemplateVersion>))]
public abstract class StrategyTemplateVersion : Audited {
    public StrategyTemplateVersionId Id { get; } = StrategyTemplateVersionId.Generate();
    public uint VersionNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string Name { get; set; }
    public StrategyFrequency Frequency { get; set; }
    public string? SourceCode { get; set; }
    public bool IsDraft { get; private set; }

    protected StrategyTemplateVersion() {
        Name = null!;
        Frequency = null!;
    }

    protected StrategyTemplateVersion(uint versionNumber, string name, StrategyFrequency frequency, string? sourceCode, bool isDraft) {
        Name = name;
        VersionNumber = versionNumber;
        Frequency = frequency;
        SourceCode = sourceCode;
        IsDraft = isDraft;
    }

    public void FinishDraft() {
        IsDraft = false;
    }

    public Strategy Instantiate(string? parametersJson) {
        var strategy = DoInstantiate(parametersJson);
        strategy.Load();
        return strategy;
    }

    protected abstract Strategy DoInstantiate(string? parametersJson);
}

