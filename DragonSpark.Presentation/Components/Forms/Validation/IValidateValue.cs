using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public interface IValidateValue<in T> : ICondition<T> {}
}