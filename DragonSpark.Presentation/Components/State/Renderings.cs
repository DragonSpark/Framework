using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences.Collections;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State;

sealed class Renderings : Membership<IRenderAware>, ICommand
{
	readonly ICollection<IRenderAware> _collection;
	readonly ArrayPool<IRenderAware>   _pool;

	public Renderings() : this([], ArrayPool<IRenderAware>.Shared) {}

	public Renderings(ICollection<IRenderAware> collection, ArrayPool<IRenderAware> pool) : base(collection)
	{
		_collection = collection;
		_pool  = pool;
	}

	public void Execute(None parameter)
	{
		using var renderings = _collection.AsValueEnumerable().ToArray(_pool);
		foreach (var i in renderings)
		{
			i.Execute();
		}
	}
}