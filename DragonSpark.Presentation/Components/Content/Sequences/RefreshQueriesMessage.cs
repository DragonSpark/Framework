using DragonSpark.Application.Entities.Queries.Runtime;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public sealed record RefreshQueriesMessage<T>(IQueries<T> Subject);