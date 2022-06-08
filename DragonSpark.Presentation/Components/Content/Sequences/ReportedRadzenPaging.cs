using DragonSpark.Model.Operations.Allocated;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public class ReportedRadzenPaging<T> : ReportedAllocated<LoadDataArgs>, IRadzenPaging<T>
{
	readonly IRadzenPaging<T> _previous;

	public ReportedRadzenPaging(IRadzenPaging<T> previous, Action<Task> report) : base(previous, report)
		=> _previous = previous;

	public ulong Count => _previous.Count;

	public IEnumerable<T>? Current => _previous.Current;
}