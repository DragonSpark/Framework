using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model;

class ChallengingModelBinder : IModelBinder
{
	readonly IReturnLocation _return;
	readonly string          _name;

	[UsedImplicitly]
	protected ChallengingModelBinder(IReturnLocation @return) : this(@return, ProviderName.Default) {}

	protected ChallengingModelBinder(IReturnLocation @return, string name)
	{
		_return = @return;
		_name   = name;
	}

	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var provider = bindingContext.ValueProvider.Get(_name);
		bindingContext.Result = provider != null
			                        ? ModelBindingResult.Success(new Challenging(provider, _return.Get(bindingContext)))
			                        : ModelBindingResult.Failed();
		return Task.CompletedTask;
	}
}