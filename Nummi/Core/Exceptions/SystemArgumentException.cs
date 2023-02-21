namespace Nummi.Core.Exceptions; 

public class SystemArgumentException : SystemException {
    
    public SystemArgumentException(string message) : base(message) { }

    public SystemArgumentException(string message, Exception causedBy) : base(message, causedBy) { }
}