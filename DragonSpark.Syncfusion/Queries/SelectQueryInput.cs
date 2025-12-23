using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class SelectQueryInput : ISelect<Stop<DataManagerRequest>, Stop<PageInput>>
{
	public static SelectQueryInput Default { get; } = new();

	SelectQueryInput() : this(DataManagerRequests.Default) {}

	readonly IAssign<PageInput, DataManagerRequest> _assign;

	public SelectQueryInput(IAssign<PageInput, DataManagerRequest> assign) => _assign = assign;

	public Stop<PageInput> Get(Stop<DataManagerRequest> parameter)
	{
		var (subject, stop) = parameter;
		var input = new PageInput(subject.RequiresCounts, null, null, subject.Skip > 0 || subject.Take > 0
			                                                              ? new(subject.Skip > 0 ? subject.Skip : null,
			                                                                    subject.Take > 0 ? subject.Take : null)
			                                                              : null);
		_assign.Execute(new(input, subject));
		return new(input, stop);
	}
}

// TODO
sealed class DataManagerRequests : ReferenceValueTable<PageInput, DataManagerRequest>
{
	public static DataManagerRequests Default { get; } = new();

	DataManagerRequests() {}
}