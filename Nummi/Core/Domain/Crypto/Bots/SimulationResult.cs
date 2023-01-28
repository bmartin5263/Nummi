using System.ComponentModel.DataAnnotations.Schema;
using KSUID;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Crypto.Bots.Thread;
using Nummi.Core.Domain.Crypto.Strategies;

namespace Nummi.Core.Domain.Crypto.Bots; 

[Table("SimulationResult")]
[PrimaryKey(nameof(Id))]
public class SimulationResult {
    
    public string Id { get; }
    
    public SimulationStatus Status { set; get; }
    
    public List<StrategyLog> Logs { get; private set; } = new();
    
    public SimulationResult() {
        Id = Ksuid.Generate().ToString();
    }

    public void AddLogs(IEnumerable<StrategyLog> logs) {
        Logs.AddRange(logs);
    }
}