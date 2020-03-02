using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Forms
{
	public interface IValidationDefinition : IOperationResult<FieldValidator, ValidationResult>
	{
		bool IsActive { get; }

		FieldValidationMessages Messages { get; }
	}
}