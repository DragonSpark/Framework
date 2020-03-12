using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class CallbackContextBinder<T> : IModelBinder where T : class
	{
		static ModelBindingResult Redirect(string message, string origin)
			=> ModelBindingResult.Success(new LoginErrorRedirect(message, origin));

		readonly SignInManager<T>  _authentication;
		readonly Text.Text         _returnUrl, _errorMessage;
		readonly IUrlHelperFactory _urls;

		[UsedImplicitly]
		public CallbackContextBinder(IUrlHelperFactory urls, SignInManager<T> authentication)
			: this(urls, authentication, ReturnUrl.Default, RemoteError.Default) {}

		// ReSharper disable once TooManyDependencies
		internal CallbackContextBinder(IUrlHelperFactory urls, SignInManager<T> authentication, Text.Text returnUrl,
		                               Text.Text errorMessage)
		{
			_authentication = authentication;
			_urls           = urls;
			_returnUrl      = returnUrl;
			_errorMessage   = errorMessage;
		}

		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var origin = bindingContext.Value(_returnUrl) ?? _urls.GetUrlHelper(bindingContext.ActionContext)
			                                                      .Content("~/");

			var error = bindingContext.Value(_errorMessage);

			if (error != null)
			{
				bindingContext.Result = Redirect($"Error from external provider: {error}", origin);
				return;
			}

			var login = await _authentication.GetExternalLoginInfoAsync();
			if (login == null)
			{
				bindingContext.Result = Redirect("Error loading external login information.", origin);
				return;
			}

			var instance = new CallbackContext(login, origin);

			bindingContext.Result = ModelBindingResult.Success(instance);
		}
	}
}