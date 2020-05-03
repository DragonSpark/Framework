using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security
{
	public sealed class Identities : ISelect<string, ProviderIdentity>
	{
		public static Identities Default { get; } = new Identities();

		Identities() : this(KeyDelimiter.Default) {}

		readonly char _token;

		public Identities(char token) => _token = token;

		public ProviderIdentity Get(string parameter)
		{
			var parts  = parameter.Split(_token);
			var result = new ProviderIdentity(parts[0], parts[1]);
			return result;
		}
	}
}