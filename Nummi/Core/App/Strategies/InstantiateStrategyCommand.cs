using System.Text.Json;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Util;

namespace Nummi.Core.App.Strategies;

public record InstantiateStrategyParameters {
    public required Ksuid StrategyTemplateId { get; init; }
    public JsonDocument? StrategyParameters { get; init; }
}

public class InstantiateStrategyCommand {
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }

    public InstantiateStrategyCommand(IStrategyTemplateRepository strategyTemplateRepository) {
        StrategyTemplateRepository = strategyTemplateRepository;
    }

    public Strategy Execute(InstantiateStrategyParameters args) {
        Strategy strategy = InstantiateStrategy(args.StrategyTemplateId, args.StrategyParameters);
        return strategy;
    }
    
    private Strategy InstantiateStrategy(Ksuid templateId, JsonDocument? jsonParameters) {
        StrategyTemplate template = StrategyTemplateRepository.FindById(templateId);
        StrategyTemplateVersion latestVersion = template.Versions[0];
        return latestVersion.Instantiate(
            parametersJson: jsonParameters == null ? null : Serializer.DocumentToJson(jsonParameters)
        );
    }
}