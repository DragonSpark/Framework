using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class ValidationOperation<T> : IStopAware<ValidationContext>
{
	readonly IValidatingValue<T> _validator;

	public ValidationOperation(IValidatingValue<T> validator) => _validator = validator;

	public async ValueTask Get(Stop<ValidationContext> parameter)
	{
		var (((context, field), messages, (invalid, _, _)), stop) = parameter;
		if (context.IsValid() && !await _validator.Off(new(field.GetValue<T>(), stop)))
		{
			messages.Add(in field, invalid);
		}
	}
}