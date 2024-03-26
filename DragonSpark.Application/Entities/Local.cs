using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities;

public sealed class Local<TKey, T> : ISelecting<TKey, T?> where T : class
{
	readonly IScopes                 _context;
	readonly Func<T, TKey>           _key;
	readonly IEqualityComparer<TKey> _equality;

	public Local(IScopes context, Func<T, TKey> key) : this(context, key, EqualityComparer<TKey>.Default) {}

	public Local(IScopes context, Func<T, TKey> key, IEqualityComparer<TKey> equality)
	{
		_context  = context;
		_key      = key;
		_equality = equality;
	}

	public ValueTask<T?> Get(TKey parameter)
	{
		var subject = _context.Get();
		foreach (var local in subject.Set<T>().Local.AsValueEnumerable())
		{
			var entity = _key(local);
			if (_equality.Equals(entity, parameter))
			{
				return local.ToOperation<T?>();
			}
		}

		return default(T?).ToOperation();
	}
}