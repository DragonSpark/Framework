using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Compose
{
	// TODO: Remove
	public sealed class ValidationDefinitionContext
	{
		readonly ISelecting<FieldIdentifier, bool> _operation;

		public ValidationDefinitionContext(ISelecting<FieldIdentifier, bool> operation)
			=> _operation = operation;

		public IValidationDefinition WithMessaging(string invalid, string loading = "Loading...",
		                                           string error = "There was a problem validating this field.")
			=> new ValidationDefinition(_operation.AsView(), new FieldValidationMessages(invalid, loading, error));
	}
}