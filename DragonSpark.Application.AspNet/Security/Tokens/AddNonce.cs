using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Runtime;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class AddNonce : IStopAware<IssueNonceInput, string>
{
    readonly Editors _editors;
    readonly IText   _nonce;
    readonly ITime   _time;

    public AddNonce(Editors editors) : this(editors, DefaultFormattedTokens.Default, Time.Default) {}

    public AddNonce(Editors editors, IText nonce, ITime time)
    {
        _editors = editors;
        _nonce   = nonce;
        _time    = time;
    }

    public async ValueTask<string> Get(Stop<IssueNonceInput> parameter)
    {
        var ((context, type), stop) = parameter;
        using var editor = _editors.Get(stop);
        var       result = _nonce.Get();
        var       now    = _time.Get().UtcDateTime;

        editor.Add(new Nonce
        {
            Key          = result,
            Purpose      = type,
            Scope        = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
            IssuedAtUtc  = now,
            ExpiresAtUtc = now + DefaultExpiration.Default
        });

        await editor.Off();
        return result;
    }
}