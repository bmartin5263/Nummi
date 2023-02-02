namespace Nummi.Core.Exceptions; 

public class InvalidSystemArgumentException : SystemException {
    
    public InvalidSystemArgumentException(string message) : base(message) { }
    
}