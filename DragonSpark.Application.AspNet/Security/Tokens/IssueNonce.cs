using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class IssueNonce : IIssueNonce
{
    readonly AddNonce            _add;
    readonly ILogger<IssueNonce> _logger;

    public IssueNonce(AddNonce add, ILogger<IssueNonce> logger)
    {
        _add    = add;
        _logger = logger;
    }

    public async ValueTask<string> Get(IssueNonceInput parameter)
    {
        try
        {
            return await _add.Off(new(parameter, parameter.Context.RequestAborted));
        }
        catch (DbUpdateException ex) when (IsDuplicate.Default.Get(ex))
        {
            _logger.LogWarning("Nonce collision; regenerating");
            return await _add.Off(new(parameter, parameter.Context.RequestAborted));
        }
    }
}