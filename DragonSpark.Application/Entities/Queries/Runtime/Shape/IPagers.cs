using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public interface IPagers<T> : ISelect<PagingInput<T>, IPaging<T>> {}