using System.Text.Json;
using Nummi.Core.App.Strategies;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.App.Commands;

public record ActivateBotParameters {
    public required BotId BotId { get; init; }
    public required StrategyTemplateId StrategyTemplateId { get; init; }
    public JsonDocument? StrategyJsonParameters { get; init; }
}

public class ActivateBotCommand {
    private IBotRepository BotRepository { get; }
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    private InstantiateStrategyCommand InstantiateStrategyCommand { get; }
    
    public ActivateBotCommand(IBotRepository botRepository, IStrategyTemplateRepository strategyTemplateRepository, InstantiateStrategyCommand instantiateStrategyCommand) {
        BotRepository = botRepository;
        StrategyTemplateRepository = strategyTemplateRepository;
        InstantiateStrategyCommand = instantiateStrategyCommand;
    }

    public BotActivation Execute(ActivateBotParameters args) {
        var bot = BotRepository.FindById(args.BotId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(args.BotId));

        if (bot.IsActive) {
            throw new InvalidUserArgumentException("Bot is already active");
        }

        if (bot.InErrorState) {
            throw new InvalidUserArgumentException("Cannot activate a Bot in error state");
        }
        
        Strategy strategy = InstantiateStrategyCommand.Execute(new InstantiateStrategyParameters {
            StrategyParameters = args.StrategyJsonParameters,
            StrategyTemplateId = args.StrategyTemplateId
        });
        
        var activation = bot.Activate(strategy); // Domain Event BotActivated
        BotRepository.Commit();

        return activation;
    }
    
}