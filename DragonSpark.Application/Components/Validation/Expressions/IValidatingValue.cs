using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Components.Validation.Expressions;

public interface IValidatingValue<in T> : IDepending<T> {}