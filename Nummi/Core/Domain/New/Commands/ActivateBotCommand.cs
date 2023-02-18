using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record ActivateBotParameters {
    public required Ksuid StrategyTemplateId { get; init; }
    public string? JsonParameters { get; init; }
}

public class ActivateBotCommand {
    private ITransaction Transaction { get; }
    
    public ActivateBotCommand(ITransaction transaction) {
        Transaction = transaction;
    }

    public BotActivation Execute(Ksuid botId, ActivateBotParameters parameters) {
        var bot = Transaction.BotRepository.FindById(botId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));

        if (bot.IsActive) {
            return bot.CurrentActivation!;
        }
        
        var strategyTemplate = Transaction.StrategyTemplateRepository.FindById(parameters.StrategyTemplateId)
            .OrElseThrow(() => EntityNotFoundException<Strategy>.IdNotFound(parameters.StrategyTemplateId));
        var strategy = strategyTemplate.Instantiate(parameters.JsonParameters);

        var activation = bot.Activate(strategy); // Domain Event BotActivated
        Transaction.SaveAndDispose();

        return activation;
    }
    
}