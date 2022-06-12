using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities;

public sealed class Local<TKey, T> : ISelecting<T, T?> where T : class
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

	public async ValueTask<T?> Get(T parameter)
	{
		var key = _key(parameter);
		var (subject, boundary) = _context.Get();
		using var _ = await boundary.Await();
		foreach (var local in subject.Set<T>().Local.AsValueEnumerable())
		{
			var entity = _key(local);
			if (_equality.Equals(entity, key))
			{
				return local;
			}
		}

		return null;
	}
}