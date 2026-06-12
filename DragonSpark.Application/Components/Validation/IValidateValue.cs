using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Components.Validation;

public interface IValidateValue<in T> : ICondition<T?>;