using System.Text.Json;
using Nummi.Core.App.Simulations;
using Nummi.Core.Domain.Common;
using Nummi.Core.Util;

namespace Nummi.Api.Model; 

public record SimulateStrategyParametersDto {
    public required string StrategyTemplateId { get; init; }
    public required DateTimeOffset StartTime { get; init; }
    public required DateTimeOffset EndTime { get; init; }
    public required decimal Funds { get; init; }
    public JsonDocument? StrategyJsonParameters { get; init; }

    public SimulateStrategyParameters ToDomain(Ksuid userId) {
        return new SimulateStrategyParameters {
            StartTime = StartTime,
            EndTime = EndTime,
            Funds = Funds,
            StrategyTemplateId = StrategyTemplateId.ToKsuid(),
            StrategyJsonParameters = StrategyJsonParameters,
            UserId = userId
        };
    }
}