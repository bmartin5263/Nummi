namespace Nummi.Core.Bridge; 

public interface INummiScope : IDisposable {
    public T GetScoped<T>() where T : notnull;
}