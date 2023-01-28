using System.Net;

namespace Nummi.Core.Exceptions; 

public class InvalidArgumentException : NummiException {
    
    public InvalidArgumentException(string message, HttpStatusCode code = HttpStatusCode.BadRequest) : base(message, code) {
        
    }
    
}