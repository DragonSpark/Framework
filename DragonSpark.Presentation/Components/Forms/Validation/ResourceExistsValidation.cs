using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ResourceExistsValidation : IValidatingValue<string>
{
	readonly IResourceQuery _query;

	public ResourceExistsValidation(IResourceQuery query) => _query = query;

	public async ValueTask<bool> Get(Stop<string> parameter) => await _query.Off(parameter) is not null;
}