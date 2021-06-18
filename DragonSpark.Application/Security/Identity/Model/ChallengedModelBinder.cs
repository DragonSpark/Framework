using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ChallengedModelBinder : IModelBinder
	{
		readonly ChallengedModel _model;

		public ChallengedModelBinder(ChallengedModel model) => _model = model;

		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			bindingContext.Result = ModelBindingResult.Success(await _model.Await(bindingContext));
		}
	}
}