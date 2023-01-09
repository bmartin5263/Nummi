using Nummi.Core.Domain.Stocks.Data;

namespace Nummi.Core.Domain.Stocks.Analysis; 

public class BarAnalyzer {
    private IEnumerable<IBar> dataset;

    public BarAnalyzer(IEnumerable<IBar> dataset) {
        this.dataset = dataset;
    }
    
}