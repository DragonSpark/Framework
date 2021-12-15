using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public readonly record struct PreRenderingAwarePagersInput<T>(IPaging<T> Paging, IFormatter<QueryInput> Formatter);