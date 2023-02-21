using Nummi.Core.Config;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.New;

namespace Nummi.Core.Domain.Strategies;

public class ReInitializeBuiltinStrategiesCommand {
    
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }
    private StrategyInstantiator StrategyInstantiator { get; }
    
    public ReInitializeBuiltinStrategiesCommand(IStrategyTemplateRepository strategyTemplateRepository, StrategyInstantiator strategyInstantiator) {
        StrategyTemplateRepository = strategyTemplateRepository;
        StrategyInstantiator = strategyInstantiator;
    }

    public IEnumerable<StrategyTemplate> Execute() {
        StrategyTemplateRepository.RemoveAllByUserId(Configuration.ADMIN_USER_ID);
        var instanceInterfaceType = typeof(IStrategyLogicBuiltin);
        var instanceTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => instanceInterfaceType.IsAssignableFrom(p) && (!p.IsInterface || !p.IsAbstract))
            .ToList();

        var templates = new List<StrategyTemplate>();
        foreach (Type instanceType in instanceTypes) {
            var strategyLogic = (IStrategyLogicBuiltin) Activator.CreateInstance(instanceType)!;
            StrategyTemplate template = StrategyInstantiator.CreateBuiltinTemplate(strategyLogic);
            StrategyTemplateRepository.Add(template);
            templates.Add(template);
        }

        StrategyTemplateRepository.Commit();
        return templates;
    }
    
}