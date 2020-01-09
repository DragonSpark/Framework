using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Compose.Model
{
	public class AlterationSelector<T> : Selector<T, T>
	{
		public static implicit operator Func<T, T>(AlterationSelector<T> instance) => instance.Get().Get;

		public AlterationSelector(IAlteration<T> subject) : base(subject) {}
	}
}