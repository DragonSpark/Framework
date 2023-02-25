using DragonSpark.Model.Commands;
using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime;

sealed class LargeOrdered<T> : ICommand<IEnumerable<T>> where T : class, ILargeOrderAware
{
	public static LargeOrdered<T> Default { get; } = new();

	LargeOrdered() {}

	public void Execute(IEnumerable<T> parameter)
	{
		var order = 0u;
		foreach (var aware in parameter.AsValueEnumerable())
		{
			aware.Order ??= order++;
		}
	}
}