using DragonSpark.Application.Security.Identity.Authentication.Persist;
using DragonSpark.Application.Security.Identity.Claims.Access;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Authenticate<T> : IAuthenticate<T> where T : IdentityUser
{
	readonly IPersistSignIn<T> _persist;
	readonly IClaims           _claims;
	readonly IReadClaim        _contact;

	public Authenticate(IPersistSignIn<T> persist, IClaims claims)
		: this(persist, claims, ContactAddressClaim.Default) {}

	public Authenticate(IPersistSignIn<T> persist, IClaims claims, IReadClaim contact)
	{
		_persist = persist;
		_claims  = claims;
		_contact = contact;
	}

	public ValueTask Get(Login<T> parameter)
	{
		var (information, user) = parameter;
		var claims  = _claims.Get(new(information.Principal, information.LoginProvider, information.ProviderKey));
		var contact = _contact.Get(information.Principal).Claim();
		var input   = contact is not null ? claims.Append(contact) : claims;
		return _persist.Get(new(user, input));
	}
}