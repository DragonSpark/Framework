using DragonSpark.Text;
using System.Linq;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ProviderIdentityParser : IParser<ProviderIdentity>
{
	public static ProviderIdentityParser Default { get; } = new();

	ProviderIdentityParser() : this('_') {}

	readonly char _token;

	public ProviderIdentityParser(char token) => _token = token;

	public ProviderIdentity Get(string parameter)
	{
		var parts = parameter.Split(_token);
		return new (parts.First(), parts.Last());
	}
}