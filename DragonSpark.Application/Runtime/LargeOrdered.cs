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

// TODO:
sealed class MediumOrdered<T> : ICommand<IEnumerable<T>> where T : class, IMediumOrderAware
{
	public static MediumOrdered<T> Default { get; } = new ();

	MediumOrdered() {}

	public void Execute(IEnumerable<T> parameter)
	{
		ushort order = 0;
		foreach (var aware in parameter.AsValueEnumerable())
		{
			aware.Order ??= order++;
		}
	}
}

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
