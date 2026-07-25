namespace DragonSpark.Presentation.Components.Content.Sequences;

public readonly record struct PagingRenderState(Exception? Error, QueryRenderState State);