namespace Nummi.Core.Exceptions; 

public class InvalidUserArgumentException : UserException {
    
    public InvalidUserArgumentException(string message) : base(message) { }

    public InvalidUserArgumentException(string message, Exception causedBy) : base(message, causedBy) { }
}