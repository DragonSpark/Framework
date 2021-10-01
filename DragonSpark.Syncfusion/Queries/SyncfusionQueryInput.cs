using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using Syncfusion.Blazor;

namespace DragonSpark.Syncfusion.Queries
{
	public sealed class SyncfusionQueryInput : QueryInput
	{
		public DataManagerRequest Request { get; set; } = default!;
	}
}