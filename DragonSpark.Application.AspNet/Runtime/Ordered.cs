using DragonSpark.Model.Commands;
using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime;

sealed class Ordered<T> : ICommand<IEnumerable<T>> where T : class, IOrderAware
{
	public static Ordered<T> Default { get; } = new ();

	Ordered() {}

	public void Execute(IEnumerable<T> parameter)
	{
		byte order = 0;
		foreach (var aware in parameter.AsValueEnumerable())
		{
			aware.Order = order++;
		}
	}
}