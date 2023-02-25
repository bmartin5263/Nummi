using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Strategies;

namespace Nummi.Core.App.Strategies; 

public class GetStrategyTemplatesQuery {
    private IStrategyTemplateRepository StrategyTemplateRepository { get; }

    public GetStrategyTemplatesQuery(IStrategyTemplateRepository strategyTemplateRepository) {
        StrategyTemplateRepository = strategyTemplateRepository;
    }

    public IEnumerable<StrategyTemplate> Execute() {
        return StrategyTemplateRepository
            .FindAll();
    }
    
}