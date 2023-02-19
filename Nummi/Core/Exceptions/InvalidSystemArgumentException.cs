namespace Nummi.Core.Exceptions; 

public class InvalidSystemArgumentException : SystemException {
    
    public InvalidSystemArgumentException(string message) : base(message) { }

    public InvalidSystemArgumentException(string message, Exception causedBy) : base(message, causedBy) { }
}