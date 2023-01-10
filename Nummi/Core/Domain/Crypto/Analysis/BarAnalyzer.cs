using Nummi.Core.Domain.Crypto.Data;

namespace Nummi.Core.Domain.Crypto.Analysis; 

public class BarAnalyzer {
    private IEnumerable<IBar> dataset;

    public BarAnalyzer(IEnumerable<IBar> dataset) {
        this.dataset = dataset;
    }
    
}