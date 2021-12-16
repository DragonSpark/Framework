namespace DragonSpark.Presentation.Components.Content.Rendering;

public readonly record struct RenderStateContentInput<T>(IActiveContent<T> Previous, string Key);