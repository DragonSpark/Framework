using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms {
	public sealed class FieldValidationDefinition
	{
		public FieldValidationDefinition(IOperationResult<FieldIdentifier, bool> operation, string errorText,
		                                 string loadingText)
		{
			Operation   = operation;
			ErrorText   = errorText;
			LoadingText = loadingText;
		}

		public string ErrorText { get; }

		public string LoadingText { get; }

		public IOperationResult<FieldIdentifier, bool> Operation { get; }
	}
}