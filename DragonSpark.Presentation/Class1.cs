using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	[AllowAnonymous, RedirectErrors]
	public class ExternalLoginModel<T> : PageModel where T : class
	{
		readonly ExternalLoginModelActions<T> _actions;

		public ExternalLoginModel(ExternalLoginModelActions<T> actions) => _actions = actions;

		public string LoginProvider { get; private set; }

		// TODO: ON???
		public IActionResult OnGet() => RedirectToPage("./Login");

		public IActionResult OnPost(ProviderContext context) => _actions.Get(context);

		public async Task<IActionResult> OnGetCallback(CallbackContext context)
			=> await _actions.Get(context) ??
			   (await _actions.Get((ModelState, context.Login))
				    ? LocalRedirect(context.Origin)
				    : Bind(context.Login));

		IActionResult Bind(UserLoginInfo login)
		{
			LoginProvider = login.LoginProvider;

			return Page();
		}
	}

	public sealed class ExternalLoginModelActions<T> : ISelect<ProviderContext, IActionResult>, IAuthenticateAction
		where T : class
	{
		readonly IAuthenticateAction _authenticate;
		readonly ICreateAction       _create;
		readonly SignInManager<T>    _authentication;

		public ExternalLoginModelActions(IAuthenticateAction authenticate, ICreateAction create,
		                                 SignInManager<T> authentication)
		{
			_authenticate   = authenticate;
			_create         = create;
			_authentication = authentication;
		}

		public IActionResult Get(ProviderContext parameter)
		{
			var (provider, returnUrl) = parameter;
			var properties = _authentication.ConfigureExternalAuthenticationProperties(provider, returnUrl);
			var result     = new ChallengeResult(provider, properties);
			return result;
		}

		public ValueTask<IActionResult> Get(CallbackContext parameter) => _authenticate.Get(parameter);

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
	}

	// TODO: Assign binder in configuration.
	public sealed class ModelBinderProvider<T> : IModelBinderProvider where T : class
	{
		public static ModelBinderProvider<T> Default { get; } = new ModelBinderProvider<T>();

		ModelBinderProvider() : this(new Dictionary<Type, Type>
		{
			[A.Type<CallbackContext>()] = A.Type<CallbackContextBinder<T>>(),
			[A.Type<ProviderContext>()] = A.Type<ProviderContextBinder>()
		}) {}

		readonly IReadOnlyDictionary<Type, Type> _types;

		public ModelBinderProvider(IReadOnlyDictionary<Type, Type> types) => _types = types;

		public IModelBinder GetBinder(ModelBinderProviderContext context)
			=> _types.TryGetValue(context.Metadata.ModelType, out var result)
				   ? context.Services.GetRequiredService(result).To<IModelBinder>()
				   : null;
	}

	sealed class CallbackContextBinder<T> : IModelBinder where T : class
	{
		readonly SignInManager<T>  _authentication;
		readonly IUrlHelperFactory _urls;
		readonly Text.Text         _returnUrl, _errorMessage;

		[UsedImplicitly]
		public CallbackContextBinder(IUrlHelperFactory urls, SignInManager<T> authentication)
			: this(urls, authentication, ReturnUrl.Default, RemoteError.Default) {}

		// ReSharper disable once TooManyDependencies
		CallbackContextBinder(IUrlHelperFactory urls, SignInManager<T> authentication, Text.Text returnUrl,
		                      Text.Text errorMessage)
		{
			_authentication = authentication;
			_urls           = urls;
			_returnUrl      = returnUrl;
			_errorMessage   = errorMessage;
		}

		static ModelBindingResult Redirect(string message, string origin)
			=> ModelBindingResult.Success(new LoginErrorRedirect(message, origin));

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

	sealed class ProviderContextBinder : IModelBinder
	{
		readonly IUrlHelperFactory _urls;
		readonly Text.Text         _provider, _returnUrl;

		[UsedImplicitly]
		public ProviderContextBinder(IUrlHelperFactory urls) : this(urls, ProviderName.Default, ReturnUrl.Default) {}

		ProviderContextBinder(IUrlHelperFactory urls, Text.Text provider, Text.Text returnUrl)
		{
			_urls      = urls;
			_provider  = provider;
			_returnUrl = returnUrl;
		}

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var returnUrl = _urls.GetUrlHelper(bindingContext.ActionContext)
			                     .Page("./ExternalLogin", "Callback",
			                           new {returnUrl = bindingContext.Value(_returnUrl)});

			var instance = new ProviderContext(bindingContext.Value(_provider), returnUrl);

			bindingContext.Result = ModelBindingResult.Success(instance);

			return Task.CompletedTask;
		}
	}

	public sealed class ReturnUrl : Text.Text
	{
		public static ReturnUrl Default { get; } = new ReturnUrl();

		ReturnUrl() : base("returnUrl") {}
	}

	sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base("provider") {}
	}

	public sealed class RemoteError : Text.Text
	{
		public static RemoteError Default { get; } = new RemoteError();

		RemoteError() : base("remoteError") {}
	}

	public static class Extensions
	{
		public static string Value(this ModelBindingContext @this, IResult<string> key)
			=> @this.ValueProvider.Get(key);

		public static string Get(this IValueProvider @this, IResult<string> key)
		{
			var name   = key.Get();
			var value  = @this.GetValue(name);
			var result = value != ValueProviderResult.None ? value.FirstValue : null;
			return result;
		}
	}

	public sealed class RedirectErrorsAttribute : Attribute, IPageFilter
	{
		public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			if (context.HandlerInstance is PageModel page &&
			    context.HandlerArguments.Only().Value is ErrorRedirect error)
			{
				page.TempData[error.Message.Key] = error.Message.Value;
				context.Result                   = page.RedirectToPage(error.Location, new {returnUrl = error.Origin});
			}
		}

		void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context) {}

		void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context) {}
	}

	public sealed class LoginErrorRedirect : ErrorRedirect
	{
		public LoginErrorRedirect(string message, string origin) : base("./Login", message, origin) {}
	}

	public class ErrorRedirect
	{
		public ErrorRedirect(string location, string message, string origin)
			: this(location, Pairs.Create("ErrorMessage", message), origin) {}

		public ErrorRedirect(string location, Pair<string, string> message, string origin)
		{
			Location = location;
			Message  = message;
			Origin   = origin;
		}

		public string Location { get; }
		public Pair<string, string> Message { get; }
		public string Origin { get; }
	}

/**/

	public sealed class CallbackContext
	{
		public CallbackContext(ExternalLoginInfo login, string origin)
		{
			Login  = login;
			Origin = origin;
		}

		public ExternalLoginInfo Login { get; }

		public string Origin { get; }

		public void Deconstruct(out ExternalLoginInfo login, out string origin)
		{
			login  = Login;
			origin = Origin;
		}
	}

	public sealed class ProviderContext
	{
		public ProviderContext(string provider, string returnUrl)
		{
			Provider  = provider;
			ReturnUrl = returnUrl;
		}

		public string Provider { get; }

		public string ReturnUrl { get; }

		public void Deconstruct(out string provider, out string returnUrl)
		{
			provider  = Provider;
			returnUrl = ReturnUrl;
		}
	}

	public interface IAuthenticateAction : IOperationResult<CallbackContext, IActionResult> {}

	sealed class AuthenticateAction<T> : IAuthenticateAction where T : IdentityUser
	{
		readonly SignInManager<T>               _authentication;
		readonly ILogger<AuthenticateAction<T>> _log;

		public AuthenticateAction(SignInManager<T> authentication, ILogger<AuthenticateAction<T>> log)
		{
			_authentication = authentication;
			_log            = log;
		}

		public async ValueTask<IActionResult> Get(CallbackContext parameter)
		{
			var (login, origin) = parameter;

			var result = await _authentication.ExternalLoginSignInAsync(login.LoginProvider,
			                                                            login.ProviderKey, false, true);
			if (result.Succeeded)
			{
				_log.LogInformation("[{Id}] {Name} logged in with {LoginProvider} provider.",
				                    login.ProviderKey, login.Principal.Identity.Name, login.LoginProvider);

				return new LocalRedirectResult(origin);
			}

			return result.IsLockedOut ? new RedirectToPageResult("./Lockout") : null;
		}
	}
}