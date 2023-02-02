namespace Nummi.Core.Exceptions; 

public abstract class SystemException : NummiException {
    protected SystemException(string message) 
        : base(message) { }

    protected SystemException(string message, Exception causedBy) : base(message, causedBy) {
        
    }
}