using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Forms
{
	// TODO: Remove.
	public interface IFieldValidator : IFieldValidator<object> {}

	public interface IFieldValidator<in T> : ISelect<T, ValidationResult> {}
}