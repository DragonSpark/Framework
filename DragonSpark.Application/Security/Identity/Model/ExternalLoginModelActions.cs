using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class ExternalLoginModelActions<T> : ISelect<Challenging, IActionResult>, IAuthenticationRequest
		where T : class
	{
		readonly IAuthenticationRequest _authenticate;
		readonly SignInManager<T>       _authentication;
		readonly ICreateRequest         _create;

		public ExternalLoginModelActions(IAuthenticationRequest authenticate, ICreateRequest create,
		                                 SignInManager<T> authentication)
		{
			_authenticate   = authenticate;
			_create         = create;
			_authentication = authentication;
		}

		public IActionResult Get(Challenging parameter)
		{
			var (provider, origin) = parameter;
			var properties = _authentication.ConfigureExternalAuthenticationProperties(provider, origin);
			var result     = new ChallengeResult(provider, properties);
			return result;
		}

		public ValueTask<IActionResult?> Get(Challenged parameter) => _authenticate.Get(parameter);

		public async ValueTask<bool> Get((ExternalLoginInfo Login, ModelStateDictionary State) parameter)
		{
			var (login, state) = parameter;
			var call   = await _create.Await(login);
			var result = call.Succeeded;

			if (!result)
			{
				foreach (var error in call.Errors.AsValueEnumerable())
				{
					state.AddModelError(string.Empty, error!.Description);
				}
			}

			return result;
		}
	}
}