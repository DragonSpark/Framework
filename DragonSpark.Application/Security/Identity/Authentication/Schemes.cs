using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public class Schemes<T> : IResulting<Array<string>> where T : class
	{
		readonly SignInManager<T> _authentication;

		public Schemes(SignInManager<T> authentication) => _authentication = authentication;

		public async ValueTask<Array<string>> Get()
		{
			var schemes = await _authentication.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false);
			var result  = schemes.AsValueEnumerable().Select(x => x.Name).ToArray();
			return result;
		}
	}
}