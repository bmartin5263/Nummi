using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Common;
using Nummi.Core.Domain.New;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public class StrategyTemplateRepository : GenericRepository<Ksuid, StrategyTemplate>, IStrategyTemplateRepository {
    public StrategyTemplateRepository(ITransaction context) : base(context) { }

    public void RemoveAllByUserId(Ksuid userId) {
        var templates = Context.StrategyTemplates
            .Include(x => x.Versions)
            .Where(x => x.UserId == userId)
            .ToList();

        foreach (var template in templates) {
            template.Versions.Clear();
        }

        Context.StrategyTemplates.RemoveRange(templates);
    }

    public StrategyTemplate? FindByUserAndName(Ksuid userId, string name) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .MaxBy(v => v.VersionNumber)
            )
            .FirstOrDefault(t => t.Name == name && t.UserId == userId);
    }

    public override StrategyTemplate? FindNullableById(Ksuid id) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .MaxBy(v => v.VersionNumber)
            )
            .FirstOrDefault(t => t.Id == id);
    }

    public override StrategyTemplate FindById(Ksuid id) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .MaxBy(v => v.VersionNumber)
            )
            .FirstOrDefault(t => t.Id == id)
            .OrElseThrow(() => EntityNotFoundException<StrategyTemplate>.IdNotFound(id));
    }

    public override StrategyTemplate RequireById(Ksuid id) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .MaxBy(v => v.VersionNumber)
            )
            .FirstOrDefault(t => t.Id == id)
            .OrElseThrow(() => new EntityMissingException<StrategyTemplate>(id));
    }

    public override IEnumerable<StrategyTemplate> FindAll() {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .MaxBy(v => v.VersionNumber)
            );
    }
}