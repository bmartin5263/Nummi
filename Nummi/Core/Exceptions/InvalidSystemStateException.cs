namespace Nummi.Core.Exceptions; 

public class InvalidSystemStateException : SystemException {
    
    public InvalidSystemStateException(string message) : base(message) { }
    
}