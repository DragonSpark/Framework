using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class SelectQueryInput : ISelect<DataManagerRequest, PageInput>
{
	public static SelectQueryInput Default { get; } = new();

	SelectQueryInput() : this(DataManagerRequests.Default) {}

	readonly IAssign<PageInput, DataManagerRequest> _assign;

	public SelectQueryInput(IAssign<PageInput, DataManagerRequest> assign) => _assign = assign;

	public PageInput Get(DataManagerRequest parameter)
	{
		var result = new PageInput(parameter.RequiresCounts, null, null,
		                           parameter.Skip > 0 || parameter.Take > 0
			                           ? new(parameter.Skip > 0 ? parameter.Skip : null,
			                                 parameter.Take > 0 ? parameter.Take : null)
			                           : null);
		_assign.Execute(new(result, parameter));
		return result;
	}
}