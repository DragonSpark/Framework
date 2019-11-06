using System;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Selection.Alterations
{
	sealed class Alterations<T> : ReferenceValueStore<Func<T, T>, IAlteration<T>>
	{
		public static Alterations<T> Default { get; } = new Alterations<T>();

		Alterations() : base(x => x.Target as IAlteration<T> ?? new Alteration<T>(x)) {}
	}
}