using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class AddNonce<T> : IStopAware<HttpRequest, string> where T : Nonce
{
    readonly Editors                 _editors;
    readonly ISelect<HttpRequest, T> _new;

    public AddNonce(Editors editors) : this(editors, NewNonce<T>.Default) {}

    public AddNonce(Editors editors, ISelect<HttpRequest, T> @new)
    {
        _editors = editors;
        _new     = @new;
    }

    public async ValueTask<string> Get(Stop<HttpRequest> parameter)
    {
        var (request, stop) = parameter;
        using var editor = _editors.Get(stop);
        var       @new   = _new.Get(request);
        editor.Add(@new);
        await editor.Off();
        return @new.Key;
    }
}