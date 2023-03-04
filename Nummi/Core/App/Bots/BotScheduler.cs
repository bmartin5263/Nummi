using System.Collections.Immutable;
using NLog;
using Nummi.Core.Domain.Bots;

namespace Nummi.Core.App.Bots; 

public class BotScheduler {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private SortedSet<BotNode> BotExecutionQueue { get; }
    private HashSet<BotId> ScheduledBots { get; }

    public BotScheduler() {
        BotExecutionQueue = new SortedSet<BotNode>(BotNode.ExecuteAtComparer);
        ScheduledBots = new HashSet<BotId>();
    }

    public void Schedule(IList<BotNode> botIds) {
        BotExecutionQueue.UnionWith(botIds.Where(node => !ScheduledBots.Contains(node.BotId)));
        ScheduledBots.UnionWith(botIds.Select(b => b.BotId));
    }

    public void Unschedule(ISet<BotId> removeIds) {
        BotExecutionQueue.RemoveWhere(node => removeIds.Contains(node.BotId));
        ScheduledBots.ExceptWith(removeIds);
    }

    public IEnumerable<BotId> FindBotsReadyForExecution(BotExecutorContext context) {
        if (BotExecutionQueue.Count == 0) {
            Log.Info("No Bots to Schedule");
            return ImmutableList<BotId>.Empty;
        }

        var bots = new List<BotId>();
        var executeUntil = context.Now;

        while (BotExecutionQueue.Count > 0) {
            BotNode nextNode = BotExecutionQueue.Min;
            if (nextNode.ExecuteAt >= executeUntil) {
                break;
            }
            BotExecutionQueue.Remove(nextNode);
            ScheduledBots.Remove(nextNode.BotId);

            bots.Add(nextNode.BotId);
        }

        return bots;
    }
}

public readonly record struct BotNode(BotId BotId, DateTimeOffset ExecuteAt) {

    private sealed class ExecuteAtRelationalComparer : IComparer<BotNode> {
        public int Compare(BotNode x, BotNode y) {
            var executeAtComparison = x.ExecuteAt.CompareTo(y.ExecuteAt);
            return executeAtComparison != 0 
                ? executeAtComparison 
                : string.Compare(x.BotId.ToString(), y.BotId.ToString(), StringComparison.Ordinal);
        }
    }

    public static IComparer<BotNode> ExecuteAtComparer { get; } = new ExecuteAtRelationalComparer();
}