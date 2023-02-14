using Nummi.Core.Domain.New;

namespace Nummi.Core.Database.Common; 

public interface IBarRepository {
    Bar? FindById(string symbol, DateTimeOffset openTime, TimeSpan period);
    List<Bar> FindByIdRange(string symbol, DateTimeOffset startOpenTime, DateTimeOffset endOpenTime, TimeSpan period);
    void Add(Bar bar);
    int AddRange(IEnumerable<Bar> bars);
    void Save();
}