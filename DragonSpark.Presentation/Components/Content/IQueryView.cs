using DragonSpark.Model.Operations;
using Radzen;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content
{
	public interface IQueryView<out T> : IAllocated<LoadDataArgs>
	{
		IEnumerable<T> Current { get; }

		ulong Count { get; }
	}
}