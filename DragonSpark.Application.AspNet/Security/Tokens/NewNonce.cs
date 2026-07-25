using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class NewNonce<T> : ISelect<HttpRequest, T> where T : Nonce
{
    public static NewNonce<T> Default { get; } = new();

    NewNonce() : this(New<T>.Default.Get, DefaultFormattedTokens.Default, Time.Default) {}

    readonly Func<T> _new;
    readonly IText   _nonce;
    readonly ITime   _time;

    public NewNonce(Func<T> @new, IText nonce, ITime time)
    {
        _new   = @new;
        _nonce = nonce;
        _time  = time;
    }

    public T Get(HttpRequest parameter)
    {
        var now    = _time.Get().UtcDateTime;
        var result = _new();
        result.Key          = _nonce.Get();
        result.Scope        = $"{parameter.Scheme}://{parameter.Host}{parameter.Path}";
        result.IssuedAtUtc  = now;
        result.ExpiresAtUtc = now + DefaultExpiration.Default;
        return result;
    }
}