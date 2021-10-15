using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public interface ICompose<T> : ISelecting<ComposeInput<T>, Composition<T>> {}