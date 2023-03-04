using System.Diagnostics.CodeAnalysis;

namespace Nummi.Core.Domain.Bots; 

public readonly record struct BotLogId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public static BotLogId Generate() => new(Guid.NewGuid());
    public static BotLogId FromGuid(Guid id) => new(id);
    public static BotLogId FromString(string s) => new(Guid.ParseExact(s, "N"));
}

public class BotLog {
    public BotLogId Id { get; } = BotLogId.Generate();
    
    public required DateTime StartTime { get; init; }
    
    public required DateTime EndTime { get; init; }

    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan TotalTime {
        get => EndTime - StartTime;
        private init { }
    }
    
    public string? Error { get; init; }
}