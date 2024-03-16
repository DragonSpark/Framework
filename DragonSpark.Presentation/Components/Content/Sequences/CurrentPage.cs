using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public sealed class CurrentPage<T> : Variable<Page<T>>;