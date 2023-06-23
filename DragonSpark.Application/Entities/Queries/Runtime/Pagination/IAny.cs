using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IAny<T> : IDepending<AnyInput<T>> {}