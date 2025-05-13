using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Communication.Http;

public abstract class User<T> : ISelect<ClaimsPrincipal, T>
{
	readonly ISelect<ClaimsPrincipal, string?> _bearer;
	readonly Func<T>                           _client;
	readonly IMutable<string?>                 _store;

	protected User(ApplicationUserAwareBearer bearer, Func<T> client)
		: this(bearer, client, AmbientBearer.Default) {}

	protected User(ISelect<ClaimsPrincipal, string?> bearer, Func<T> client, IMutable<string?> store)
	{
		_bearer = bearer;
		_client = client;
		_store  = store;
	}

	public T Get(ClaimsPrincipal parameter)
	{
		var bearer = _bearer.Get(parameter);
		using (_store.Assigned(bearer))
		{
			return _client();
		}
	}
}