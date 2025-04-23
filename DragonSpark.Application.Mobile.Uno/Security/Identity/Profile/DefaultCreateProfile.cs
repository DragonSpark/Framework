using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity.Profile;

sealed class DefaultCreateProfile : ICreateProfile
{
	public static DefaultCreateProfile Default { get; } = new();

	DefaultCreateProfile() : this(Identifier.Default, FirstName.Default, LastName.Default, FullName.Default) {}

	readonly IRequiredClaim _identifier, _first, _last, _full;

	// ReSharper disable once TooManyDependencies
	public DefaultCreateProfile(IRequiredClaim identifier, IRequiredClaim first, IRequiredClaim last,
	                            IRequiredClaim full)
	{
		_identifier = identifier;
		_first      = first;
		_last       = last;
		_full       = full;
	}

	public Profile Get(ClaimsPrincipal parameter)
		=> new(_identifier.Get(parameter), _first.Get(parameter), _last.Get(parameter), _full.Get(parameter));
}