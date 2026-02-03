using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Tokens;

sealed class ApplyToken : IStopAware<ApplyTokenInput, HttpRequestMessage?>
{
    readonly ITokens                          _tokens;
    readonly NewRequest                       _new;
    readonly ISelect<HttpRequestMessage, Uri> _origin;

    public ApplyToken(ITokens tokens, NewRequest @new) : this(tokens, @new, Origins.Default) {}

    public ApplyToken(ITokens tokens, NewRequest @new, ISelect<HttpRequestMessage, Uri> origin)
    {
        _tokens = tokens;
        _new    = @new;
        _origin = origin;
    }

    public ValueTask<HttpRequestMessage?> Get(Stop<ApplyTokenInput> parameter)
    {
        var ((request, response, token), stop) = parameter;

        var origin = _origin.Get(request);
        _tokens.Execute((origin, token));

        return response.StatusCode == HttpStatusCode.Unauthorized
                   ? _new.Accounting(new(request, stop))
                   : ValueTask.FromResult<HttpRequestMessage?>(null);
    }
}