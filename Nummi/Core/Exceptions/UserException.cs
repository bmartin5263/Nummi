namespace Nummi.Core.Exceptions; 

public abstract class UserException : NummiException {
    protected UserException(string message) 
        : base(message) { }

    protected UserException(string message, Exception causedBy) : base(message, causedBy) {
        
    }
}