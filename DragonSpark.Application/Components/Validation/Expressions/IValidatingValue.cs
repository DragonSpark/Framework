using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Components.Validation.Expressions;

public interface IValidatingValue<in T> : IDepending<T> {}