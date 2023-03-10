using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.Common;
using Nummi.Core.Domain.Strategies;
using Nummi.Core.Domain.User;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public class StrategyTemplateRepository : GenericRepository<StrategyTemplateId, StrategyTemplate>, IStrategyTemplateRepository {
    public StrategyTemplateRepository(ITransaction context) : base(context) { }

    public void RemoveAllByUserId(IdentityId userId) {
        var templates = Context.StrategyTemplates
            .Include(x => x.Versions)
            .Where(x => x.UserId == userId)
            .ToList();

        foreach (var template in templates) {
            template.Versions.Clear();
        }

        Context.StrategyTemplates.RemoveRange(templates);
    }

    public StrategyTemplate? FindByUserAndName(IdentityId userId, string name) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .OrderByDescending(v => v.VersionNumber)
                .Take(1)
            )
            .FirstOrDefault(t => t.Name == name && t.UserId == userId);
    }

    public override StrategyTemplate? FindNullableById(StrategyTemplateId id) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .OrderByDescending(v => v.VersionNumber)
                .Take(1)
            )
            .FirstOrDefault(t => t.Id == id);
    }

    public override StrategyTemplate FindById(StrategyTemplateId id) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .OrderByDescending(v => v.VersionNumber)
                .Take(1)
            )
            .FirstOrDefault(t => t.Id == id)
            .OrElseThrow(() => EntityNotFoundException<StrategyTemplate>.IdNotFound(id));
    }

    public override StrategyTemplate RequireById(StrategyTemplateId id) {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .OrderByDescending(v => v.VersionNumber)
                .Take(1)
            )
            .FirstOrDefault(t => t.Id == id)
            .OrElseThrow(() => new EntityMissingException<StrategyTemplate>(id));
    }

    public override IEnumerable<StrategyTemplate> FindAll() {
        return Context.Set<StrategyTemplate>()
            .Include(t => t.Versions
                .OrderByDescending(v => v.VersionNumber)
                .Take(1)
            );
    }
}