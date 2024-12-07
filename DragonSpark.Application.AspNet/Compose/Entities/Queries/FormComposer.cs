using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class FormComposer<TIn, T>
{
	readonly IElements<TIn, T> _subject;

	public FormComposer(IElements<TIn, T> subject) => _subject = subject;
	public IInput<TIn, Array<T>> Array() => new Input<TIn, T, Array<T>>(_subject, ToArray<T>.Default);

	public IInput<TIn, Leasing<T>> Lease() => new Input<TIn, T, Leasing<T>>(_subject, ToLease<T>.Default);

	public IInput<TIn, List<T>> List() => new Input<TIn, T, List<T>>(_subject, ToList<T>.Default);

	public IInput<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
		=> new Input<TIn, T, Dictionary<TKey, T>>(_subject, new ToDictionary<T, TKey>(key));

	public IInput<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
		Func<T, TKey> key, Func<T, TValue> value)
		where TKey : notnull
		=> new Input<TIn, T, Dictionary<TKey, TValue>>(_subject, new ToDictionary<T, TKey, TValue>(key, value));

	public IInput<TIn, T> Single() => new Input<TIn, T, T>(_subject, ToSingle<T>.Default);

	public IInput<TIn, T?> SingleOrDefault() => new Input<TIn, T, T?>(_subject, ToSingleOrDefault<T>.Default);

	public IInput<TIn, T> First() => new Input<TIn, T, T>(_subject, ToFirst<T>.Default);

	public IInput<TIn, T?> FirstOrDefault() => new Input<TIn, T, T?>(_subject, ToFirstOrDefault<T>.Default);

	public IInput<TIn, bool> Any() => new Input<TIn, T, bool>(_subject, ToAny<T>.Default);

}