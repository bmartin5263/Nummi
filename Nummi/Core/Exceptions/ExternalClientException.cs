namespace Nummi.Core.Exceptions; 

public class ExternalClientException : SystemException {
    public ExternalClientException(string message) : base(message) { }
    public ExternalClientException(string message, Exception innerException) : base(message, innerException) { }
}