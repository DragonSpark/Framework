using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class SyncfusionCompose<T> : Compose<T>
{
	public static SyncfusionCompose<T> Default { get; } = new();

	SyncfusionCompose() : base(Body<T>.Default) {}

	public SyncfusionCompose(ISelect<PageInput, DataManagerRequest> select) : base(new Body<T>(select)) {}
}