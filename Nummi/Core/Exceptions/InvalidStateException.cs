using System.Net;

namespace Nummi.Core.Exceptions; 

public class InvalidStateException : NummiException {
    
    public InvalidStateException(string message) : base(message) {
        
    }

    public InvalidStateException(string message, HttpStatusCode code = HttpStatusCode.InternalServerError) : base(message, code) { }
}