using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

public readonly record struct ActiveContentInput<T>(ComponentBase Owner, IResulting<T?> Source);