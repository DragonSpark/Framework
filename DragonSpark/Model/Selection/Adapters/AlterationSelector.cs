using System;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Model.Selection.Adapters
{
	public class AlterationSelector<T> : Selector<T, T>
	{
		public AlterationSelector(IAlteration<T> subject) : base(subject) {}

		public static implicit operator Func<T, T>(AlterationSelector<T> instance) => instance.Get().ToDelegate();
	}
}