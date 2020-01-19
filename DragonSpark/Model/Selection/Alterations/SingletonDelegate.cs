using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Model.Selection.Alterations
{
	public sealed class SingletonDelegate<T> : Alteration<Func<T>>
	{
		public static SingletonDelegate<T> Default { get; } = new SingletonDelegate<T>();

		SingletonDelegate() : base(x => new DeferredSingleton<T>(x).Get) {}
	}
}