using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Components.Validation.Expressions;

public interface IValidatingValue<T> : IDependingWithStop<T>;