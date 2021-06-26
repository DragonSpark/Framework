using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Conditions
{
	public class Equals<T> : ICondition<T>, IActivateUsing<T>
	{
		readonly IEqualityComparer<T> _comparer;

		readonly T _source;

		public Equals(T source) : this(source, EqualityComparer<T>.Default) {}

		public Equals(T source, IEqualityComparer<T> comparer)
		{
			_source   = source;
			_comparer = comparer;
		}

		public bool Get(T parameter) => _comparer.Equals(parameter, _source);
	}

	public class Equaling<T> : ICondition<T>, IActivateUsing<T>
	{ 
		readonly IEqualityComparer<T> _comparer;

		readonly Func<T> _source;

		public Equaling(Func<T> source) : this(source, EqualityComparer<T>.Default) {}

		public Equaling(Func<T> source, IEqualityComparer<T> comparer)
		{
			_source   = source;
			_comparer = comparer;
		}

		public bool Get(T parameter) => _comparer.Equals(parameter, _source());
	}
}