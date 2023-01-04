namespace TestWebApp.Core.External; 

public class ExternalClientException : Exception {
    public ExternalClientException() { }
    public ExternalClientException(string? message) : base(message) { }
    public ExternalClientException(string? message, Exception? innerException) : base(message, innerException) { }
}