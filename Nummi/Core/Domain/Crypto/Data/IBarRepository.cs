namespace Nummi.Core.Domain.Crypto.Data; 

public interface IBarRepository {
    Bar? FindById(string symbol, long openTimeUnixMs, long periodUnixMs);
    List<Bar> FindByIdRange(string symbol, long startOpenTimeUnixMs, long endOpenTimeUnixMs, long periodUnixMs);
    void Add(Bar bar);
    void Save();
}