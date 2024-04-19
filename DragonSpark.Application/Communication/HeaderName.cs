using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace DragonSpark.Application.Communication;

public class HeaderName : Text.Text
{
	protected HeaderName(HttpRequestHeader instance) : base(instance.ToString()) {}
}

// TODO

public abstract class Configure : ICommand<HttpClient>
{
	readonly Uri              _base;
	readonly IResult<string?> _bearer;

	protected Configure(IResult<Uri> connection) : this(connection.Get(), AmbientBearer.Default) {}

	protected Configure(Uri @base, IResult<string?> bearer)
	{
		_base   = @base;
		_bearer = bearer;
	}

	public void Execute(HttpClient parameter)
	{
		var bearer  = _bearer.Get();
		var headers = parameter.DefaultRequestHeaders;
		headers.Authorization = bearer is not null ? new("Bearer", bearer) : headers.Authorization;
		parameter.BaseAddress = _base;
	}
}

public abstract class UserClients : User<HttpClient>
{
	protected UserClients(ApplicationUserAwareBearer bearer, IHttpClientFactory clients, string name)
		: base(bearer, () => clients.CreateClient(name)) {}
}

public interface IUser<out T> : ISelect<ClaimsPrincipal, T>;
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