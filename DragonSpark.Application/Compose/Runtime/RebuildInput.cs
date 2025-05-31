using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Runtime;

public readonly record struct RebuildInput<T>(ICollection<T> Subject, Memory<T> Source);