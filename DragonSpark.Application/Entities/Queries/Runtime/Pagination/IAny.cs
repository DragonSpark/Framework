using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IAny<T> : IDepending<AnyInput<T>> {}