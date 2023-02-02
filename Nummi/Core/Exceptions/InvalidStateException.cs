namespace Nummi.Core.Exceptions; 

public class InvalidStateException : SystemException {
    
    public InvalidStateException(string message) : base(message) { }
    
}