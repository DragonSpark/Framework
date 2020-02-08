using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	sealed class ProviderContextBinder : IModelBinder
	{
		readonly IUrlHelperFactory _urls;
		readonly Text.Text              _provider, _returnUrl;

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

	[ModelBinder(typeof(ProviderContextBinder))]
	public sealed class ProviderContext
	{
		public ProviderContext(string provider, string returnUrl)
		{
			Provider  = provider;
			ReturnUrl = returnUrl;
		}

		public string Provider { get; }

		public string ReturnUrl { get; }
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

		public static string UniqueId(this ExternalLoginInfo @this) => $"{@this.LoginProvider}+{@this.ProviderKey}";
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
}