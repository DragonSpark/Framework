using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Selection;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public sealed class IdentityParser : ISelect<string, ProviderIdentity>
	{
		public static IdentityParser Default { get; } = new IdentityParser();

		IdentityParser() : this(KeyDelimiter.Default) {}

		readonly char _token;

		public IdentityParser(char token) => _token = token;

		public ProviderIdentity Get(string parameter)
		{
			var parts  = parameter.Split(_token);
			var result = new ProviderIdentity(parts[0], parts[1]);
			return result;
		}
	}

	public sealed class Identities : ISelect<ClaimsPrincipal, ProviderIdentity>
	{
		public static Identities Default { get; } = new Identities();

		Identities() : this(ExternalIdentity.Default, IdentityParser.Default) {}

		readonly string                            _type;
		readonly ISelect<string, ProviderIdentity> _parser;

		public Identities(string type, ISelect<string, ProviderIdentity> parser)
		{
			_type   = type;
			_parser = parser;
		}

		public ProviderIdentity Get(ClaimsPrincipal parameter)
		{
			var content = parameter.FindFirstValue(_type) ??
			              throw new
				              InvalidOperationException($"Content not found for claim {_type} in user {parameter.UserName()}.");
			var result = _parser.Get(content);
			return result;
		}
	}
}