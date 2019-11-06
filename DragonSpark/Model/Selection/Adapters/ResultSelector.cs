using System;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Selection.Adapters
{
	public class ResultSelector<T> : Selector<None, T>
	{
		public ResultSelector(ISelect<None, T> subject) : base(subject) {}

		public static implicit operator Func<T>(ResultSelector<T> instance) => instance.Get().ToResult().ToDelegate();
	}
}