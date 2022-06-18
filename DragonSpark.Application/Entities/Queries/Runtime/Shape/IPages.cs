using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public interface IPages<T> : ISelecting<PageInput, Page<T>> {}