using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

public readonly record struct PerformSelectInput<T>(IQueryable<T> Source, List<string> Columns);