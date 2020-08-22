using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	sealed class ValidationOperation<T> : IOperation<ValidationContext>
	{
		readonly IFieldValidation<T> _validator;

		public ValidationOperation(IFieldValidation<T> validator) => _validator = validator;

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