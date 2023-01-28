using System.Net;
using System.Reflection;

namespace Nummi.Core.Exceptions; 

public class EntityNotFoundException : NummiException {
    public EntityNotFoundException(MemberInfo type, object id, HttpStatusCode code): 
        base($"{type.Name} not found by id: {id}", code) 
    { }
    
    public EntityNotFoundException(string message): 
        base(message) 
    { }

    public EntityNotFoundException(string message, HttpStatusCode code = HttpStatusCode.InternalServerError) :
        base(message, code) {
    }
}