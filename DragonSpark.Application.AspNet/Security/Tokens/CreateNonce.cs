using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class CreateNonce<T> : ISelecting<HttpContext, string> where T : Nonce
{
    readonly AddNonce<T>             _add;
    readonly ILogger<CreateNonce<T>> _logger;

    public CreateNonce(AddNonce<T> add, ILogger<CreateNonce<T>> logger)
    {
        _add    = add;
        _logger = logger;
    }

    public async ValueTask<string> Get(HttpContext parameter)
    {
        var stop = parameter.Request.Stop(parameter.RequestAborted);
        try
        {
            return await _add.Off(stop);
        }
        catch (DbUpdateException ex) when (IsDuplicate.Default.Get(ex))
        {
            _logger.LogWarning("Nonce collision; regenerating");
            return await _add.Off(stop);
        }
    }
}