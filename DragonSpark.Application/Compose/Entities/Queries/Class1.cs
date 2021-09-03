using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	class Class1 {}

	public sealed class EditInvocationComposer<TIn, T>
	{
		readonly IInvoke<TIn, T> _invoke;

		public EditInvocationComposer(IInvoke<TIn, T> invoke) => _invoke = invoke;

		public IEdit<TIn, Array<T>> Array() => new Edit<TIn, T, Array<T>>(_invoke, ToArray<T>.Default);

		public IEdit<TIn, Lease<T>> Lease() => new Edit<TIn, T, Lease<T>>(_invoke, ToLease<T>.Default);

		public IEdit<TIn, List<T>> List() => new Edit<TIn, T, List<T>>(_invoke, ToList<T>.Default);

		public IEdit<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new Edit<TIn, T, Dictionary<TKey, T>>(_invoke, new ToDictionary<T, TKey>(key));

		public IEdit<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new Edit<TIn, T, Dictionary<TKey, TValue>>(_invoke, new ToDictionary<T, TKey, TValue>(key, value));

		public IEdit<TIn, T> Single() => new Edit<TIn, T, T>(_invoke, Single<T>.Default);

		public IEdit<TIn, T?> SingleOrDefault() => new Edit<TIn, T, T?>(_invoke, SingleOrDefault<T>.Default);

		public IEdit<TIn, T> First() => new Edit<TIn, T, T>(_invoke, First<T>.Default);

		public IEdit<TIn, T?> FirstOrDefault() => new Edit<TIn, T, T?>(_invoke, FirstOrDefault<T>.Default);
	}
}