using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Bots;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public class BotRepository : GenericRepository<BotId, Bot>, IBotRepository {
    public BotRepository(ITransaction context) : base(context) { }

    public Bot FindByIdWithCurrentActivation(BotId botId) {
        return Context.Bots
            .Include(b => b.ActivationHistory
                .OrderByDescending(a => a.CreatedAt)
                .Take(1)
            )
            .FirstOrDefault(b => b.Id == botId)
            .OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
    }

    public Bot? FindByIdWithStrategyAndActivation(BotId botId) {
        return Context.Bots
            .Include(b => b.ActivationHistory)
            .FirstOrDefault(b => b.Id == botId);
    }

    public Bot? FindByIdForExecution(BotId botId) {
        return Context.Bots
            .Include(b => b.ActivationHistory)
            .Include(b => b.CurrentActivation!)
                .ThenInclude(s => s.Strategy)
                    .ThenInclude(s => s.ParentTemplateVersion)
            .Include(b => b.CurrentActivation!)
                .ThenInclude(s => s.Strategy)
                    .ThenInclude(s => s.Logs
                        .OrderByDescending(log => log.StartTime)
                        .Take(1)
                    )
            .FirstOrDefault(b => b.Id == botId);
    }

    public List<Bot> FindActiveWithStrategyAndActivation() {
        return Context.Bots
            .Include(b => b.ActivationHistory)
            .Include(b => b.CurrentActivation!)
            .ThenInclude(s => s.Strategy)
            .ToList();
    }
}