using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Api.Filters; 

public class JsonExceptionMiddleware
{
    private IWebHostEnvironment Environment { get; set; }

    public JsonExceptionMiddleware(IWebHostEnvironment environment) {
        Environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        switch (ex) {
            case null:
                return;
            case NummiException exception:
                context.Response.StatusCode = (int) exception.StatusCode;
                break;
        }

        var error = new 
        {
            code = context.Response.StatusCode,
            type = ex.GetType().Name,
            message = ex.Message,
            trace = ex.StackTrace?.Split('\n')
        };

        context.Response.ContentType = "application/json";

        using (var writer = new StreamWriter(context.Response.Body))
        {
            Serializer.ToJsonAsync(writer.BaseStream, error);
            await writer.FlushAsync().ConfigureAwait(false);
        }
    }
}