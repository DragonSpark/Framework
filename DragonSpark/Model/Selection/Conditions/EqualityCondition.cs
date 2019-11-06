using System.Collections.Generic;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection.Conditions
{
	public class EqualityCondition<T> : ICondition<T>, IActivateUsing<T>
	{
		readonly IEqualityComparer<T> _comparer;

		readonly T _source;

		public EqualityCondition(T source) : this(source, EqualityComparer<T>.Default) {}

		public EqualityCondition(T source, IEqualityComparer<T> comparer)
		{
			_source   = source;
			_comparer = comparer;
		}

		public bool Get(T parameter) => _comparer.Equals(parameter, _source);
	}
}