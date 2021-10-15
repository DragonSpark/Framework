using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Components.Validation.Expressions;

public interface IValidateValue<in T> : ICondition<T> {}