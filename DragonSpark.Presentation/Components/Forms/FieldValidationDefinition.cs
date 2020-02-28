using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class FieldValidationDefinition
	{
		public FieldValidationDefinition(IOperationResult<FieldIdentifier, bool> operation, FieldValidationMessages messages)
		{
			Operation   = operation;
			Messages = messages;
		}

		public IOperationResult<FieldIdentifier, bool> Operation { get; }
		public FieldValidationMessages Messages { get; }
	}
}