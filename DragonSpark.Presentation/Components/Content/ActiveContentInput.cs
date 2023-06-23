using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Presentation.Components.Content;

public readonly record struct ActiveContentInput<T>(Microsoft.AspNetCore.Components.ComponentBase Owner,
                                                    IResulting<T?> Source);