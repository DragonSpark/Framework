using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content;

public readonly record struct SaveStateViewContext<T>(T Subject, EventCallback Save);