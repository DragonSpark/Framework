using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public readonly record struct RenderAwareActiveContentBuilderInput<T>(IActiveContent<T> Previous,
                                                                      Func<ValueTask<T?>> Content);