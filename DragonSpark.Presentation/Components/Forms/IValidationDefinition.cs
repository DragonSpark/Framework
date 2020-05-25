using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Forms
{
	public interface IValidationDefinition : ISelecting<FieldValidator, ValidationResult>
	{
		bool IsActive { get; }

		FieldValidationMessages Messages { get; }
	}
}