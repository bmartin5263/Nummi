using System.Text.Json.Serialization;
using Nummi.Core.Domain.Common;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Strategies; 

[JsonConverter(typeof(Serializer.AbstractTypeConverter<StrategyTemplateVersion>))]
public abstract class StrategyTemplateVersion : Audited {
    public uint VersionNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string Name { get; set; }
    public TimeSpan Frequency { get; set; }
    public string? SourceCode { get; set; }
    public bool IsDraft { get; private set; }

    protected StrategyTemplateVersion() {
        Name = null!;
    }

    protected StrategyTemplateVersion(uint versionNumber, string name, TimeSpan frequency, string? sourceCode, bool isDraft) {
        Name = name;
        VersionNumber = versionNumber;
        Frequency = frequency;
        SourceCode = sourceCode;
        IsDraft = isDraft;
    }

    public void FinishDraft() {
        IsDraft = false;
    }

    public abstract bool AcceptsParameters { get; }
    public abstract Strategy Instantiate(string? parameters);
}

