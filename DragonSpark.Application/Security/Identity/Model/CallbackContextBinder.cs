using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class CallbackContextBinder : IModelBinder
	{
		static ModelBindingResult Redirect(string message, string origin)
			=> ModelBindingResult.Success(new LoginErrorRedirect(message, origin));

		readonly IUrlHelperFactory      _urls;
		readonly IAuthenticationProfile _profile;
		readonly Text.Text              _returnUrl, _errorMessage;

		[UsedImplicitly]
		public CallbackContextBinder(IUrlHelperFactory urls, IAuthenticationProfile profile)
			: this(urls, profile, ReturnUrl.Default, RemoteError.Default) {}

		// ReSharper disable once TooManyDependencies
		internal CallbackContextBinder(IUrlHelperFactory urls, IAuthenticationProfile profile, Text.Text returnUrl,
		                               Text.Text errorMessage)
		{
			_urls         = urls;
			_profile      = profile;
			_returnUrl    = returnUrl;
			_errorMessage = errorMessage;
		}

		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var origin = bindingContext.Value(_returnUrl) ?? _urls.GetUrlHelper(bindingContext.ActionContext)
			                                                      .Content("~/").Verify();

			var error = bindingContext.Value(_errorMessage);

			if (error != null)
			{
				bindingContext.Result = Redirect($"Error from external provider: {error}", origin);
				return;
			}

			var login = await _profile.Get();

			bindingContext.Result = login != null
				                        ? ModelBindingResult.Success(new Challenged(login, origin))
				                        : Redirect("Error loading external login information.", origin);
		}
	}
}