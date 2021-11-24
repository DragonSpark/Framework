using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IAccessToken>()
		         .Forward<ApiAccessToken>()
		         .Decorate<ExpirationAwareAccessToken>()
		         .Decorate<ThreadAwareApiAccessToken>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<IUserIdentifierQuery>()
		         .Forward<UserIdentifierQuery>()
		         .Include(x => x.Dependencies)
		         .Singleton()
			;
	}
}

sealed class ThreadAwareApiAccessToken : Protecting<AccessToken>, IAccessToken
{
	public ThreadAwareApiAccessToken(IAccessToken source) : base(source) {}
}

sealed class ExpirationAwareAccessToken : IAccessToken
{
	readonly IMutable<AccessToken?>  _store;
	readonly IResulting<AccessToken> _previous;
	readonly ITime                   _time;

	public ExpirationAwareAccessToken(IAccessToken previous)
		: this(AccessTokenStore.Default, previous, Time.Default) {}

	public ExpirationAwareAccessToken(IMutable<AccessToken?> store, IResulting<AccessToken> previous, ITime time)
	{
		_store    = store;
		_previous = previous;
		_time     = time;
	}

	public ValueTask<AccessToken> Get()
	{
		if (_store.Get()?.Expires < _time.Get())
		{
			_store.Execute(default);
		}

		return _previous.Get();
	}
}

sealed class ApiAccessToken : Deferring<AccessToken>, IAccessToken
{
	public ApiAccessToken(GetAccessToken get) : base(AccessTokenStore.Default, get) {}
}

sealed class AccessTokenStore : Variable<AccessToken?>
{
	public static AccessTokenStore Default { get; } = new();

	AccessTokenStore() {}
}

public interface IAccessToken : IResulting<AccessToken> {}

sealed class AccessTokenLocation : Model.Results.Instance<Uri>
{
	public AccessTokenLocation(DeviantArtApplicationSettings settings)
		: this(new
			       Uri($"https://www.deviantart.com/oauth2/token?grant_type=client_credentials&client_id={settings.Key}&client_secret={settings.Secret}")) {}

	public AccessTokenLocation(Uri instance) : base(instance) {}
}

sealed class GetAccessToken : IResulting<AccessToken>
{
	readonly IHttpClientFactory                        _clients;
	readonly Uri                                       _location;
	readonly ISelect<AccessTokenResponse, AccessToken> _token;

	public GetAccessToken(IHttpClientFactory clients, AccessTokenLocation location)
		: this(clients, location, AccessTokens.Default) {}

	public GetAccessToken(IHttpClientFactory clients, Uri location, ISelect<AccessTokenResponse, AccessToken> token)
	{
		_clients  = clients;
		_location = location;
		_token    = token;
	}

	public async ValueTask<AccessToken> Get()
	{
		using var client = _clients.CreateClient();
		var response = await client.GetFromJsonAsync<AccessTokenResponse>(_location).ConfigureAwait(false)
		               ??
		               throw new InvalidOperationException();
		return _token.Get(response);
	}
}

sealed class AccessTokens : ISelect<AccessTokenResponse, AccessToken>
{
	public static AccessTokens Default { get; } = new();

	AccessTokens() : this(Error.Instance, Time.Default) {}

	readonly ITemplate<(string?, string?)> _error;
	readonly ITime                         _time;

	public AccessTokens(ITemplate<(string?, string?)> error, ITime time)
	{
		_error = error;
		_time  = time;
	}

	public AccessToken Get(AccessTokenResponse parameter)
	{
		switch (parameter.Status)
		{
			case "error":
				throw _error.Get(parameter.Error, parameter.ErrorMessage);
		}

		return new AccessToken(parameter.Token, _time.Get().AddSeconds(parameter.ExpirationInSeconds));
	}

	sealed class Error : ExceptionTemplate<string?, string?>
	{
		public static Error Instance { get; } = new();

		Error() : base("Exception encountered with communicating with DeviantArt API: {Code} - {Message}") {}
	}
}

public record AccessToken(string Token, DateTimeOffset Expires);

sealed class AccessTokenResponse : ApiResponse
{
	[JsonPropertyName("access_token")]
	public string Token { get; set; } = default!;

	[JsonPropertyName("expires_in")]
	public int ExpirationInSeconds { get; set; } = default!;
}

class ApiResponse
{
	[JsonPropertyName("status")]
	public string Status { get; set; } = default!;

	[JsonPropertyName("error")]
	public string? Error { get; set; } = default!;

	[JsonPropertyName("error_description")]
	public string? ErrorMessage { get; set; } = default!;
}

sealed class ErrorResponse : ApiResponse
{
	[JsonPropertyName("error_code")]
	public byte Number { get; set; } = default!;
}

sealed class UserResponse
{
	[JsonPropertyName("user")]
	public UserResult Result { get; set; } = default!;
}

sealed class UserResult
{
	[JsonPropertyName("userid")]
	public string UserId { get; set; } = default!;
}

sealed class UserIdentifierResponse : ISelecting<string, HttpResponseMessage>
{
	readonly IHttpClientFactory _clients;
	readonly IAccessToken       _token;
	readonly string             _template;

	public UserIdentifierResponse(IHttpClientFactory clients, IAccessToken token)
		: this(clients, token, "https://www.deviantart.com/api/v1/oauth2/user/profile/{0}?access_token={1}") {}

	public UserIdentifierResponse(IHttpClientFactory clients, IAccessToken token, string template)
	{
		_clients  = clients;
		_token    = token;
		_template = template;
	}

	public async ValueTask<HttpResponseMessage> Get(string parameter)
	{
		var       token    = await _token.Await();
		using var client   = _clients.CreateClient();
		var       location = new Uri(_template.FormatWith(parameter, token.Token));
		var       result   = await client.GetAsync(location);
		return result;
	}
}

public interface IUserIdentifierQuery : ISelecting<string, string?> {}

sealed class UserIdentifierQuery : IUserIdentifierQuery
{
	readonly UserIdentifierResponse _response;
	readonly Template               _template;

	public UserIdentifierQuery(UserIdentifierResponse response, Template template)
	{
		_response = response;
		_template = template;
	}

	public async ValueTask<string?> Get(string parameter)
	{
		var response = await _response.Await(parameter);

		if (response.IsSuccessStatusCode)
		{
			var user   = await response.Content.ReadFromJsonAsync<UserResponse>();
			var result = user?.Result.UserId;
			return result;
		}

		var body = await response.Content.ReadFromJsonAsync<ErrorResponse>() ?? throw new InvalidOperationException();
		_template.Execute(parameter, body.Error, body.ErrorMessage);

		return null;
	}

	public sealed class Template : LogWarning<string, string?, string?>
	{
		public Template(ILogger<UserIdentifierQuery> logger)
			: base(logger, "There was a problem with querying {Query}: {Code} - {Message}") {}
	}
}