using DragonSpark.Model.Commands;
using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime;

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