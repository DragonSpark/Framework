using DragonSpark.Text;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class IdentityFormatter : IFormatter<ProviderIdentity>
	{
		public static IdentityFormatter Default { get; } = new IdentityFormatter();

		IdentityFormatter() : this(KeyDelimiter.Default) {}

		readonly char _token;

		public IdentityFormatter(char token) => _token = token;

		public string Get(ProviderIdentity parameter) => $"{parameter.Provider}{_token}{parameter.Identity}";
	}

}