namespace Nummi.Core.Exceptions; 

public class AuthenticationException : NummiException {
    public AuthenticationException(string message) : base(message) { }
    public AuthenticationException(string message, Exception causedBy) : base(message, causedBy) { }
}