namespace Nummi.Core.Database.Common; 

public interface ITransaction : IDisposable {
    public void Commit();
    public object DbContext { get; }
}