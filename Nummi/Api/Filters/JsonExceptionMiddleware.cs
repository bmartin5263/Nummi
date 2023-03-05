using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;
using SystemException = System.SystemException;

namespace Nummi.Api.Filters; 

public class JsonExceptionMiddleware {
    
    public async Task Invoke(HttpContext context) {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        switch (ex) {
            case null:
                return;
            case UserException:
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                break;
            case SystemException:
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                break;
            case AuthenticationException:
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                break;
            case AuthorizationException:
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                break;
        }

        var error = new {
            code = context.Response.StatusCode,
            type = ex.GetType().Name,
            message = ex.Message,
            causedByMessage = ex.InnerException?.Message,
            trace = ex.StackTrace?.Split('\n')
        };

        context.Response.ContentType = "application/json";

        await using var writer = new StreamWriter(context.Response.Body);
        Serializer.ToJsonAsync(writer.BaseStream, error);
        await writer.FlushAsync().ConfigureAwait(false);
    }
}