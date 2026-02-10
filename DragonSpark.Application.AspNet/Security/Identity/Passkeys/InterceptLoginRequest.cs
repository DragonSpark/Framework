using System.IO;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class InterceptLoginRequest : IStopAware<InterceptLoginRequestInput>
{
    readonly IStopAware<RewriteResponseInput> _write;

    public InterceptLoginRequest(RewriteResponse write) => _write = write;

    public async ValueTask Get(Stop<InterceptLoginRequestInput> parameter)
    {
        var ((previous, context), stop) = parameter;
        var             original = context.Response.Body;
        await using var buffer   = new MemoryStream();
        context.Response.Body = buffer;
        try
        {
            await previous(context).Off();

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    buffer.Position = 0;
                    await _write.Off(new(new(context.Response, buffer), stop));
                    break;
            }

            buffer.Position = 0;
            await buffer.CopyToAsync(original, stop).Off();
        }
        finally
        {
            context.Response.Body = original;
        }
    }
}