namespace Nummi.Core.Util; 

public static class ObjectCreator {
    
    public static T Create<T>(string className) {
        Type t = Type.GetType(className)!; 
        return (T) Activator.CreateInstance(t)!;
    }
    
    public static T Create<T>(string className, params object?[]? args) {
        Type t = Type.GetType(className)!; 
        return (T) Activator.CreateInstance(t, args)!;
    }
    
}