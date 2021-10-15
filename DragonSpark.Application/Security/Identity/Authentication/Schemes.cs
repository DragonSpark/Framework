using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public class Schemes<T> : IResulting<Array<string>> where T : class
{
	readonly IAuthentications<T> _authentication;

	public Schemes(IAuthentications<T> authentication) => _authentication = authentication;

	public async ValueTask<Array<string>> Get()
	{
		using var authentication = _authentication.Get();
		var       schemes = await authentication.Subject.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false);
		var       result = schemes.AsValueEnumerable().Select(x => x.Name).ToArray();
		return result;
	}
}