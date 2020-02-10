using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	sealed class ProviderContextBinder : IModelBinder
	{
		ProviderContextBinder(IUrlHelperFactory urls, Text.Text provider, Text.Text returnUrl)
		{
			_urls      = urls;
			_provider  = provider;
			_returnUrl = returnUrl;
		}

		readonly Text.Text         _provider, _returnUrl;
		readonly IUrlHelperFactory _urls;

		[UsedImplicitly]
		public ProviderContextBinder(IUrlHelperFactory urls) : this(urls, ProviderName.Default, ReturnUrl.Default) {}

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
}