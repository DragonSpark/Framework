using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Composition;

namespace DragonSpark.Server.Security.Identity.Profile;

sealed class CurrentProfile : ICurrentProfile
{
    readonly ICurrentContext                               _context;
    readonly ContextProfiles                               _profiles;
    readonly Application.Security.Identity.Profile.Profile _default;

    public CurrentProfile(ICurrentContext context, ContextProfiles profiles)
        : this(context, profiles, DefaultProfile.Default) {}

    [Candidate(false)]
    public CurrentProfile(ICurrentContext context, ContextProfiles profiles,
                          Application.Security.Identity.Profile.Profile @default)
    {
        _context  = context;
        _profiles = profiles;
        _default  = @default;
    }

    public async ValueTask<ProfileBase> Get(CancellationToken parameter)
    {
        var context       = _context.Get();
        var authenticated = context.User.IsAuthenticated();
        var result = authenticated ? await _profiles.Off(context) : _default;
        return result;
    }
}