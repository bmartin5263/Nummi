using Nummi.Core.Database.Common;

namespace Nummi.Core.Database.EFCore; 

public class EFCoreTransaction : ITransaction {
    private bool Disposed { get; set; }
    private EFCoreContext AppDb { get; }
    public IBarRepository BarRepository { get; }
    public IBotRepository BotRepository { get; }
    public IBotThreadRepository BotThreadRepository { get; }
    public ISimulationRepository SimulationRepository { get; }
    public IStrategyRepository StrategyRepository { get; }
    public IStrategyTemplateRepository StrategyTemplateRepository { get; }
    public IUserRepository UserRepository { get; }

    public EFCoreTransaction(EFCoreContext appDb, IBarRepository barRepository, IBotRepository botRepository, IBotThreadRepository botThreadRepository, ISimulationRepository simulationRepository, IStrategyRepository strategyRepository, IStrategyTemplateRepository strategyTemplateRepository, IUserRepository userRepository) {
        AppDb = appDb;
        BarRepository = barRepository;
        BotRepository = botRepository;
        BotThreadRepository = botThreadRepository;
        SimulationRepository = simulationRepository;
        StrategyRepository = strategyRepository;
        StrategyTemplateRepository = strategyTemplateRepository;
        UserRepository = userRepository;
    }

    public void SaveChanges() {
        AppDb.SaveChanges();
    }

    public void SaveAndDispose() {
        SaveChanges();
        Dispose();
    }
    
    public void Dispose() {
        if (Disposed) {
            return;
        }
        GC.SuppressFinalize(this);
        SaveChanges();
        Disposed = true;
    }

#if DEBUG
    ~EFCoreTransaction() {
        if (!Disposed) {
            throw new ApplicationException("Transaction not disposed...");
        }
    }
#endif
}