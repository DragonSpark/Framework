using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public interface IPaging<T> : ISelecting<QueryInput, Current<T>> {}