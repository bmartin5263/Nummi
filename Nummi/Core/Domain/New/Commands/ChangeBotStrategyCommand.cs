using KSUID;
using Nummi.Core.Database.Common;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.New.Commands;

public record ChangeBotStrategyParameters {
    public required Ksuid BotId { get; init; }
    public required Ksuid StrategyTemplateId { get; init; }
    public string? JsonParameters { get; init; }
}

public class ChangeBotStrategyCommand {
    private ITransaction Transaction { get; }
    
    public ChangeBotStrategyCommand(ITransaction transaction) {
        Transaction = transaction;
    }

    public void Execute(ChangeBotStrategyParameters parameters) {
        var bot = Transaction.BotRepository.FindById(parameters.BotId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(parameters.BotId));

        if (!bot.IsActive) {
            throw new InvalidUserOperationException("Cannot change strategy of inactive bot");
        }
        
        var strategyTemplate = Transaction.StrategyTemplateRepository.FindById(parameters.StrategyTemplateId)
            .OrElseThrow(() => EntityNotFoundException<Strategy>.IdNotFound(parameters.StrategyTemplateId));
        var strategy = strategyTemplate.Instantiate(parameters.JsonParameters);
        
        bot.ChangeActiveStrategy(strategy);
        Transaction.SaveAndDispose();
    }
    
}