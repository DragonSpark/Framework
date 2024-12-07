using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Runtime;

public readonly record struct CopyListInput<TList, T>(Memory<T> Source, TList Destination) where TList : List<T>;