namespace Nummi.Core.Exceptions; 

public class InvalidUserOperationException : UserException {
    
    public InvalidUserOperationException(string message) : base(message) { }
    
}