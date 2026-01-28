using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Runtime;
using DragonSpark.Text;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Tokens;

// TODO

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<NonceCleanupService>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.Start<IMarkUsed>()
                 .Forward<MarkUsed>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IIssueNonce>()
                 .Forward<IssueNonce>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.AddHostedService<NonceCleanupService>();
    }
}

sealed class AddNonce : IStopAware<IssueNonceInput, string>
{
    readonly Editors _editors;
    readonly IText   _nonce;
    readonly ITime   _time;

    public AddNonce(Editors editors) : this(editors, DefaultFormattedNonces.Default, Time.Default) {}

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