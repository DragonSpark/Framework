using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public class AddClaimToCurrent<T> : IStopAware<IdentityResult> where T : class
{
    readonly ICurrent<T>                   _current;
    readonly IStopAware<T, IdentityResult> _claim;

    protected AddClaimToCurrent(ICurrent<T> current, IStopAware<T, IdentityResult> claim)
    {
        _current = current;
        _claim   = claim;
    }

    public async ValueTask<IdentityResult> Get(CancellationToken parameter)
    {
        var current = await _current.Off();
        var result  = await _claim.Off(new(current, parameter));
        return result;
    }
}