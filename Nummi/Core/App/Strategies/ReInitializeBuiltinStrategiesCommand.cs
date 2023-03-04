using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.App.Strategies;

public class ReInitializeBuiltinStrategiesCommand {
    
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    private StrategyTemplateFactory StrategyTemplateFactory { get; }
    
    public ReInitializeBuiltinStrategiesCommand(IStrategyTemplateRepository strategyTemplateRepository, StrategyTemplateFactory strategyTemplateFactory) {
        StrategyTemplateRepository = strategyTemplateRepository;
        StrategyTemplateFactory = strategyTemplateFactory;
    }

    public IEnumerable<StrategyTemplate> Execute() {
        var logicInstances = CreateBuiltinLogicInstances();
        var templates = new List<StrategyTemplate>();
        
        foreach (IStrategyLogicBuiltin logicInstance in logicInstances) {
            if (!StrategyTemplateRepository.ExistsById(StrategyTemplateId.FromGuid(logicInstance.Id))) {
                StrategyTemplate template = StrategyTemplateFactory.CreateBuiltinTemplate(logicInstance);
                StrategyTemplateRepository.Add(template);
                templates.Add(template);
            }
        }

        StrategyTemplateRepository.Commit();
        return templates;
    }

    private static List<IStrategyLogicBuiltin> CreateBuiltinLogicInstances() {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IStrategyLogicBuiltin).IsAssignableFrom(p) && (!p.IsInterface || !p.IsAbstract))
            .Select(t => (IStrategyLogicBuiltin) Activator.CreateInstance(t)!)
            .ToList();
    }
    
}