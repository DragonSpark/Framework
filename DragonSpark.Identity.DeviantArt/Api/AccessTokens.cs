using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Identity.DeviantArt.Api;

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