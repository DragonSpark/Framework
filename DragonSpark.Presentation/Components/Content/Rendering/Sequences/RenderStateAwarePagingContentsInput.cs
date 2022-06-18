using DragonSpark.Application.Entities.Queries.Runtime.Pagination;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

public readonly record struct RenderStateAwarePagingContentsInput<T>(object Owner, IPages<T> Previous);