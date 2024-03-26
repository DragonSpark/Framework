using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities;

public sealed class Local<TKey, T> : ISelecting<TKey, T?> where T : class
{
	readonly IScopes                 _scope;
	readonly Func<T, TKey>           _key;
	readonly IEqualityComparer<TKey> _equality;

	public Local(IScopes scope, Func<T, TKey> key) : this(scope, key, EqualityComparer<TKey>.Default) {}

	public Local(IScopes scope, Func<T, TKey> key, IEqualityComparer<TKey> equality)
	{
		_scope  = scope;
		_key      = key;
		_equality = equality;
	}

	public ValueTask<T?> Get(TKey parameter)
	{
		var (subject, boundary) = _scope.Get();
		using (boundary)
		{
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
}