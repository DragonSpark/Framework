using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public interface IBody<T> : ISelecting<ComposeInput<T>, IQueryable<T>>;