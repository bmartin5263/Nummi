using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;

namespace Nummi.Core.App.Commands;

public record ChangeBotStrategyParameters {
    public required BotId BotId { get; init; }
    public required StrategyTemplateId StrategyTemplateId { get; init; }
    public string? JsonParameters { get; init; }
}

public class ChangeBotStrategyCommand {
    private IBotRepository BotRepository { get; }
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    
    public ChangeBotStrategyCommand(IBotRepository botRepository, IStrategyTemplateRepository strategyTemplateRepository) {
        BotRepository = botRepository;
        StrategyTemplateRepository = strategyTemplateRepository;
    }

    public void Execute(ChangeBotStrategyParameters parameters) {
        var bot = BotRepository.FindById(parameters.BotId);

        if (!bot.IsActive) {
            throw new InvalidUserOperationException("Cannot change strategy of inactive bot");
        }

        StrategyTemplate template = StrategyTemplateRepository.FindById(parameters.StrategyTemplateId);
        StrategyTemplateVersion latestVersion = template.Versions[0];

        var strategy = latestVersion.Instantiate(parameters.JsonParameters);
        
        bot.ChangeActiveStrategy(strategy);
        BotRepository.Commit();
    }
    
}