using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class AllowUnassignedTextAwareValidatingValue : IValidatingValue<string>
{
	readonly IValidatingValue<string> _previous;

	public AllowUnassignedTextAwareValidatingValue(IValidatingValue<string> previous) => _previous = previous;

	public ValueTask<bool> Get(Stop<string> parameter)
		=> string.IsNullOrEmpty(parameter) ? true.ToOperation() : _previous.Get(parameter);
}