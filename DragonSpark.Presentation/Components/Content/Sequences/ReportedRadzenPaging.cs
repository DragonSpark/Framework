using DragonSpark.Model.Operations.Stop;
using Radzen;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public class ReportedRadzenPaging<T> : Reported<LoadDataArgs>, IRadzenPaging<T>
{
	readonly IRadzenPaging<T> _previous;

	public ReportedRadzenPaging(IRadzenPaging<T> previous, Action<Task> report) : base(previous, report)
		=> _previous = previous;

	public ulong Count => _previous.Count;

	public IEnumerable<T>? Current => _previous.Current;
}