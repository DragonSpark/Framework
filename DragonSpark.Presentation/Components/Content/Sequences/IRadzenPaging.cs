using DragonSpark.Model.Operations;
using Radzen;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Sequences
{
	public interface IRadzenPaging<out T> : IAllocated<LoadDataArgs>
	{
		public ulong Count { get; }

		public IEnumerable<T>? Current { get; }
	}
}