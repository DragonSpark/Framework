using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public interface ICompose<T> : IStopAware<ComposeInput<T>, Composition<T>>;