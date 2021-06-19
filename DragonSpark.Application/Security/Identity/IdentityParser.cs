using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Identity
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
}