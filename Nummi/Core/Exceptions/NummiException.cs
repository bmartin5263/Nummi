using System.Net;

namespace Nummi.Core.Exceptions; 

public class NummiException : Exception {
    
    public HttpStatusCode StatusCode { get; private init; }
    
    public NummiException(string message, HttpStatusCode code = HttpStatusCode.InternalServerError) : base(message) {
        StatusCode = code;
    }
}