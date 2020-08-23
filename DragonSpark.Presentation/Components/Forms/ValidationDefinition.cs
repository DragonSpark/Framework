using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	// TODO: Remove
	sealed class ValidationDefinition : IValidationDefinition
	{
		readonly OperationView<FieldIdentifier, bool> _view;

		public ValidationDefinition(OperationView<FieldIdentifier, bool> view, FieldValidationMessages messages)
		{
			_view    = view;
			Messages = messages;
		}

		public bool IsActive => _view.IsActive;

		public FieldValidationMessages Messages { get; }

		public async ValueTask<ValidationResult> Get(FieldValidator parameter)
		{
			var call   = await _view.Get(parameter.Identifier);
			var result = call ? ValidationResult.Success : new ValidationResult(false, Messages.Invalid);
			return result;
		}
	}
}