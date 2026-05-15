using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public abstract class AddPasskey<T> : IAddPasskey where T : class
{
    readonly IAuthentications<T> _sessions;
    readonly ICurrentPrincipal   _user;

    protected AddPasskey(IAuthentications<T> sessions, ICurrentPrincipal user)
    {
        _sessions = sessions;
        _user     = user;
    }

    public async ValueTask<AddPasskeyResult> Get(Stop<AddPasskeyInput> parameter)
    {
        var ((subject, _), _) = parameter;
        using var session     = _sessions.Get();
        var       attestation = await session.Subject.PerformPasskeyAttestationAsync(subject).Off();
        if (attestation.Succeeded)
        {
            var (_, users) = session;
            var user = await users.GetUserAsync(_user.Get()).Off();
            var add  = await session.Users.AddOrUpdatePasskeyAsync(user.Verify(), attestation.Passkey).Off();
            return add.Succeeded
                       ? new AddedPasskeyResult(attestation.Passkey)
                       : new FailedAddPasskeyResult("Error: The passkey could not be added to your account.");
        }

        return new
            FailedAddPasskeyResult($"Error: Could not add the passkey: {attestation.Failure?.Message ?? "General Error"}");
    }
}