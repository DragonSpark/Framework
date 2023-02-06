using Syncfusion.Blazor;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

public readonly record struct Parameter<T>(DataManagerRequest Request, IQueryable<T> Query, uint? Count = null);