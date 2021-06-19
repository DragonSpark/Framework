using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ChallengedModel : ISelecting<ModelBindingContext, object>
	{
		readonly IReturnLocation        _return;
		readonly IAuthenticationProfile _profile;
		readonly string                 _errorMessage;

		[UsedImplicitly]
		public ChallengedModel(ReturnOrRoot @return, IAuthenticationProfile profile)
			: this(@return, profile, RemoteError.Default) {}

		public ChallengedModel(IReturnLocation @return, IAuthenticationProfile profile, string errorMessage)
		{
			_return       = @return;
			_profile      = profile;
			_errorMessage = errorMessage;
		}

		public async ValueTask<object> Get(ModelBindingContext parameter)
		{
			var @return = _return.Get(parameter);

			var error = parameter.ValueProvider.Get(_errorMessage);

			if (error != null)
			{
				return new LoginErrorRedirect($"Error from external provider: {error}", @return);
			}

			var login = await _profile.Await();

			var result = login != null
				             ? new Challenged(login, @return)
				             : (object)new LoginErrorRedirect("Error loading external login information.", @return);
			return result;
		}
	}
}