using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public class LoadAwareComposeUserDataInputs<T> : IComposeUserDataInputs<T> where T : class
{
    readonly IComposeUserDataInputs<T> _previous;
    readonly IEnlistedScopes           _scopes;
    readonly string                    _name;

    protected LoadAwareComposeUserDataInputs(IComposeUserDataInputs<T> previous, IEnlistedScopes scopes, string name)
    {
        _name     = name;
        _previous = previous;
        _scopes   = scopes;
    }

    public async ValueTask<ComposeUserDataInput<T>> Get(CancellationToken parameter)
    {
        var       result = await _previous.Off(parameter);
        using var scopes = _scopes.Get();
        await scopes.Owner.Entry(result.User.Verify()).Navigation(_name).LoadAsync(parameter).Off();
        return result;
    }
}