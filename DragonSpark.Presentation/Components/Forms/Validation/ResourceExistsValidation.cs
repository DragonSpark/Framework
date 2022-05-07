using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ResourceExistsValidation : IValidatingValue<string>
{
	readonly IResourceQuery _query;

	public ResourceExistsValidation(IResourceQuery query) => _query = query;

	public async ValueTask<bool> Get(string parameter) => await _query.Await(parameter) is not null;
}