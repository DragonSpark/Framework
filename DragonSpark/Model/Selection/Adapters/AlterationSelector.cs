using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Model.Selection.Adapters
{
	public class AlterationSelector<T> : Selector<T, T>
	{
		public AlterationSelector(IAlteration<T> subject) : base(subject) {}

		public static implicit operator Func<T, T>(AlterationSelector<T> instance) => instance.Get().ToDelegate();
	}
}