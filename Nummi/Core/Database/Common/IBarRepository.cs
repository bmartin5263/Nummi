using Nummi.Core.Domain.Crypto;

namespace Nummi.Core.Database.Common;

public interface IBarRepository : IGenericRepository<BarId, Bar> {
    List<Bar> FindByIdRange(string symbol, DateTimeOffset startOpenTime, DateTimeOffset endOpenTime, TimeSpan period);
}