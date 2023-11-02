using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public interface IAnyComposer<T> : ISelect<IAny<T>, IAny<T>?>;