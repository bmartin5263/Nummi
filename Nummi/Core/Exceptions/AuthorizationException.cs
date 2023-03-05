namespace Nummi.Core.Exceptions; 

public class AuthorizationException : NummiException {
    public AuthorizationException(string message) : base(message) { }
    public AuthorizationException(string message, Exception causedBy) : base(message, causedBy) { }
}