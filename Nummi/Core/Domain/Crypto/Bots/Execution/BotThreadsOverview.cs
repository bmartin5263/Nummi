namespace Nummi.Core.Domain.Crypto.Bots.Execution; 

public class BotThreadsOverview {
    public uint NumThreads { get; }
    public List<BotThreadDetail> Threads { get; }

    public BotThreadsOverview(uint numThreads, List<BotThreadDetail> threads) {
        NumThreads = numThreads;
        Threads = threads;
    }
}