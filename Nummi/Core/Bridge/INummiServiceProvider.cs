namespace Nummi.Core.Bridge; 

public interface INummiServiceProvider {
    public T GetSingleton<T>();
    public INummiScope CreateScope();
}