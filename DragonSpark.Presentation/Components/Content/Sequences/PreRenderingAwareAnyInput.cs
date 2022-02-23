using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public readonly record struct PreRenderingAwareAnyInput<T>(IDepending<IQueries<T>> Previous, string Key);