using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Application.Runtime.Operations;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public sealed class RadzenReporter<T> : IReporter<IPaging<T>, IRadzenPaging<T>>
{
	public static RadzenReporter<T> Default { get; } = new();

	RadzenReporter() : this(true) {}

	readonly bool _includeCount;

	public RadzenReporter(bool includeCount = true) => _includeCount = includeCount;

	public IRadzenPaging<T> Get(Report<IPaging<T>> parameter)
	{
		var (paging, report) = parameter;
		var first  = new RadzenPaging<T>(paging, _includeCount);
		var result = new ReportedRadzenPaging<T>(first, report);
		return result;
	}
}