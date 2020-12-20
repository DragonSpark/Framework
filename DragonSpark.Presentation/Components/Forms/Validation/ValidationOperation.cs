using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	sealed class ValidationOperation<T> : IOperation<ValidationContext>
	{
		readonly IValidatingValue<T> _validator;

		public ValidationOperation(IValidatingValue<T> validator) => _validator = validator;

		public async ValueTask Get(ValidationContext parameter)
		{
			var ((_, field), messages, (invalid, _, _)) = parameter;
			if (!await _validator.Await(field.GetValue<T>()))
			{
				messages.Add(in field, invalid);
			}
		}
	}
}