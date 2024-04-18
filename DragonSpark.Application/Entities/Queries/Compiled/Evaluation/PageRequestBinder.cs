using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class PageRequestBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var provider = bindingContext.ValueProvider;
		var count    = provider.GetValue(nameof(PageRequest.Count));
		var top      = provider.GetValue(nameof(PageRequest.Top));
		var skip     = provider.GetValue(nameof(PageRequest.Skip));
		var request = new PageRequest(count.Length > 0,
		                              top.Length > 0 && byte.TryParse(top.FirstValue, out var t) ? t : null,
		                              skip.Length > 0 && uint.TryParse(skip.FirstValue, out var s) ? s : null,
		                              provider.GetValue(nameof(PageRequest.Filter)).FirstValue,
		                              provider.GetValue(nameof(PageRequest.OrderBy)).FirstValue
		                             );
		bindingContext.Result = ModelBindingResult.Success(request);
		return Task.CompletedTask;
	}
}