using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Forms
{
	public interface IFieldValidator : IFieldValidator<object> {}

	public interface IFieldValidator<in T> : ISelect<T, ValidationResult> {}
}