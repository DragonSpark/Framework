using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Compose
{
	public sealed class ValidationContext
	{
		public static ValidationContext Default { get; } = new ValidationContext();

		ValidationContext() {}

		public ValidationDefinitionContext Using(IOperationResult<FieldIdentifier, bool> operation)
			=> new ValidationDefinitionContext(operation);
	}
}
