using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class DefaultCreateProfile : ICreateProfile
{
    public static DefaultCreateProfile Default { get; } = new();

    DefaultCreateProfile()
        : this(Identifier.Default, UserName.Default, Address.Default, FirstName.Default, LastName.Default,
               FullName.Default) {}

    readonly IRequiredClaim _identifier, _handle, _address, _first, _last, _full;

    // ReSharper disable once TooManyDependencies
    public DefaultCreateProfile(IRequiredClaim identifier, IRequiredClaim handle, IRequiredClaim address,
                                IRequiredClaim first, IRequiredClaim last,
                                IRequiredClaim full)
    {
        _identifier = identifier;
        _handle     = handle;
        _address    = address;
        _first      = first;
        _last       = last;
        _full       = full;
    }

    public ProfileBase Get(ClaimsPrincipal parameter)
        => new Profile(_identifier.Get(parameter), _handle.Get(parameter), _address.Get(parameter),
                       _first.Get(parameter), _last.Get(parameter), _full.Get(parameter));
}