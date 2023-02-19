namespace Nummi.Core.Bridge; 

public interface INummiServiceProvider {
    public T GetService<T>();
    public INummiScope CreateScope();
}