using DragonSpark.Model.Operations.Allocated;
using Radzen;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public interface IRadzenPaging<out T> : IAllocatedStopAware<LoadDataArgs>
{
	public ulong Count { get; }

	public IEnumerable<T>? Current { get; }
}