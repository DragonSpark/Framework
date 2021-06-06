using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ExternalSignin<T> : IExternalSignin where T : class
	{
		readonly SignInManager<T> _authentication;

		public ExternalSignin(SignInManager<T> authentication) => _authentication = authentication;

		public ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
			=> _authentication.ExternalLoginSignInAsync(parameter.LoginProvider, parameter.ProviderKey, true, true)
			                  .ToOperation();
	}
}