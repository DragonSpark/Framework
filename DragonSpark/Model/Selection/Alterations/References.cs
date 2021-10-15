using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Model.Selection.Alterations;

sealed class References<T> : ReferenceValueStore<Func<T, T>, IAlteration<T>>
{
	public static References<T> Default { get; } = new References<T>();

	References() : base(x => x.Target as IAlteration<T> ?? new Alteration<T>(x)) {}
}