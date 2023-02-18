using System.Diagnostics.CodeAnalysis;
using Nummi.Core.Domain.Common;

namespace Nummi.Core.Domain.New; 

public class BotLog {
    public Ksuid Id { get; } = Ksuid.Generate();
    
    public required DateTime StartTime { get; init; }
    
    public required DateTime EndTime { get; init; }

    [SuppressMessage("ReSharper", "ValueParameterNotUsed", Justification = "Needed for Get-only properties")]
    public TimeSpan TotalTime {
        get => EndTime - StartTime;
        private init { }
    }
    
    public string? Error { get; init; }
}