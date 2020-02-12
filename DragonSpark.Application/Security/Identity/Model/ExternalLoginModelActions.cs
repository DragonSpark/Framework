using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class ExternalLoginModelActions<T> : ISelect<ProviderContext, IActionResult>, IAuthenticateAction
		where T : class
	{
		readonly IAuthenticateAction _authenticate;
		readonly SignInManager<T>    _authentication;
		readonly ICreateAction       _create;

		public ExternalLoginModelActions(IAuthenticateAction authenticate, ICreateAction create,
		                                 SignInManager<T> authentication)
		{
			_authenticate   = authenticate;
			_create         = create;
			_authentication = authentication;
		}

		public async ValueTask<bool> Get((ModelStateDictionary State, ExternalLoginInfo Login) parameter)
		{
			var (state, login) = parameter;
			var call   = await _create.Get(login);
			var result = call.Succeeded;

			if (!result)
			{
				foreach (var error in call.Errors)
				{
					state.AddModelError(string.Empty, error.Description);
				}
			}

			return result;
		}

		public ValueTask<IActionResult> Get(CallbackContext parameter) => _authenticate.Get(parameter);

		public IActionResult Get(ProviderContext parameter)
		{
			var (provider, origin) = parameter;
			var properties = _authentication.ConfigureExternalAuthenticationProperties(provider, origin);
			var result     = new ChallengeResult(provider, properties);
			return result;
		}
	}
}