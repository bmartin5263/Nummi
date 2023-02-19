namespace Nummi.Core.Database.Common; 

public interface ITransaction : IDisposable {
    // public IBarRepository BarRepository { get; }
    // public IBotRepository BotRepository { get; }
    // public ISimulationRepository SimulationRepository { get; }
    // public IStrategyRepository StrategyRepository { get; }
    // public IStrategyTemplateRepository StrategyTemplateRepository { get; }
    // public IUserRepository UserRepository { get; }

    public void Commit();
    public object DbContext { get; }
}