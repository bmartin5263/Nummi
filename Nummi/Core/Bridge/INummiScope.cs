namespace Nummi.Core.Bridge; 

public interface INummiScope : IDisposable {
    public T GetService<T>();
}