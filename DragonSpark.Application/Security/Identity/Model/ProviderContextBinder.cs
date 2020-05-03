using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ProviderContextBinder : IModelBinder
	{
		readonly Text.Text         _provider, _returnUrl;
		readonly IUrlHelperFactory _urls;

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

			var instance = new ProviderContext(bindingContext.Value(_provider).Verify(), returnUrl);

			bindingContext.Result = ModelBindingResult.Success(instance);

			return Task.CompletedTask;
		}
	}
}