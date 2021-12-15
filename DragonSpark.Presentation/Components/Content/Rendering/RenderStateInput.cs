namespace DragonSpark.Presentation.Components.Content.Rendering;

public readonly record struct RenderStateInput<T>(T Parameter, string Key, RenderState State);