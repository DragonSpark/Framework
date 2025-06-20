using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class CurrentProfile : ICurrentProfile
{
    readonly ICurrentPrincipal _principal;
    readonly ICreateProfile    _profile;
    readonly Profile           _default;

    public CurrentProfile(ICurrentPrincipal principal, ICreateProfile profile)
        : this(principal, profile, DefaultProfile.Default) {}

    public CurrentProfile(ICurrentPrincipal principal, ICreateProfile profile, Profile @default)
    {
        _principal = principal;
        _profile   = profile;
        _default   = @default;
    }

    public ValueTask<Profile> Get(CancellationToken parameter)
    {
        var principal     = _principal.Get();
        var authenticated = principal.IsAuthenticated();
        var profile       = authenticated ? _profile.Get(new(principal, parameter)).To<Profile>() : _default;
        var result        = profile.ToOperation();
        return result;
    }
}