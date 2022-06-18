using DragonSpark.Application.Entities.Queries.Runtime.Pagination;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

public readonly record struct RenderStateAwareAnyInput<T>(object Owner, IAny<T> Previous);