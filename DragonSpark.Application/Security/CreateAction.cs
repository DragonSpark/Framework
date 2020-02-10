using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	sealed class CreateAction<T> : ICreateAction where T : IdentityUser
	{
		readonly SignInManager<T>         _authentication;
		readonly ICreateUser<T>           _create;
		readonly ILogger<CreateAction<T>> _log;

		public CreateAction(ICreateUser<T> create, SignInManager<T> authentication, ILogger<CreateAction<T>> log)
		{
			_create         = create;
			_authentication = authentication;
			_log            = log;
		}

		public async ValueTask<IdentityResult> Get(ExternalLoginInfo parameter)
		{
			var (user, result) = await _create.Get(parameter);
			if (result.Succeeded)
			{
				_log.LogInformation("User {UserName} created an account using {Provider} provider.",
				                    user.UserName, parameter.LoginProvider);

				await _authentication.SignInAsync(user, false);
			}

			return result;
		}
	}
}