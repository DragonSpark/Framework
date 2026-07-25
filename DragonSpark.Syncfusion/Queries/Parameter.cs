using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

public readonly record struct Parameter<T>(DataManagerRequest Request, IQueryable<T> Query);