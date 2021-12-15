using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public readonly record struct PagersInput<T>(PagingInput<T> Input, IFormatter<QueryInput> Formatter);