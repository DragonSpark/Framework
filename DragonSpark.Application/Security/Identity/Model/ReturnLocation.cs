using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DragonSpark.Application.Security.Identity.Model;

public class ReturnLocation : IReturnLocation
{
	readonly IUrlHelperFactory   _factory;
	readonly IPagePathDefinition _path;

	protected ReturnLocation(IUrlHelperFactory factory, IPagePathDefinition path)
	{
		_factory = factory;
		_path    = path;
	}

	public string Get(ModelBindingContext parameter)
		=> _path.Get(_factory.GetUrlHelper(parameter.ActionContext), parameter.ValueProvider);
}