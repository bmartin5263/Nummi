using Nummi.Core.Database.Common;

namespace Nummi.Core.Domain.Strategies; 

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