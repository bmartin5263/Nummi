using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record ActivateBotParameters {
    public required string StrategyTemplateId { get; init; }
    public string? JsonParameters { get; init; }
}

public class ActivateBotCommand {
    private IBotRepository BotRepository { get; }
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    
    public ActivateBotCommand(IBotRepository botRepository, IStrategyTemplateRepository strategyTemplateRepository) {
        BotRepository = botRepository;
        StrategyTemplateRepository = strategyTemplateRepository;
    }

    public BotActivation Execute(Ksuid botId, ActivateBotParameters parameters) {
        var bot = BotRepository.FindById(botId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));

        if (bot.IsActive) {
            return bot.CurrentActivation!;
        }

        var strategyTemplate = StrategyTemplateRepository.FindById(parameters.StrategyTemplateId.ToKsuid());
        var strategy = strategyTemplate.Instantiate(parameters.JsonParameters);

        var activation = bot.Activate(strategy); // Domain Event BotActivated
        BotRepository.Commit();

        return activation;
    }
    
}