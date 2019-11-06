using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DragonSpark.Model.Sequences.Query.Construction;

namespace DragonSpark.Model.Sequences.Query
{
	static class Build
	{
		public sealed class Concatenation<T> : Builder<T, T, ISequence<T>>
		{
			public Concatenation(ISequence<T> parameter)
				: base((body, stores, sequence, limit) => new Query.Concatenation<T>(body, sequence, stores, limit),
				       parameter) {}
		}

		public sealed class Union<T> : IContents<T, T>
		{
			readonly IEqualityComparer<T> _comparer;
			readonly ISequence<T>         _others;

			public Union(ISequence<T> others, IEqualityComparer<T> comparer)
			{
				_others   = others;
				_comparer = comparer;
			}

			public IContent<T, T> Get(Parameter<T, T> parameter)
				=> new Query.Union<T>(parameter.Body, _others, _comparer, parameter.Stores, parameter.Limit);
		}

		public sealed class Intersect<T> : IContents<T, T>
		{
			readonly IEqualityComparer<T> _comparer;
			readonly ISequence<T>         _others;

			public Intersect(ISequence<T> others, IEqualityComparer<T> comparer)
			{
				_others   = others;
				_comparer = comparer;
			}

			public IContent<T, T> Get(Parameter<T, T> parameter)
				=> new Query.Intersect<T>(parameter.Body, _others, _comparer, parameter.Stores, parameter.Limit);
		}

		public class SelectMany<TIn, TOut> : Builder<TIn, TOut, Func<TIn, IEnumerable<TOut>>>
		{
			public SelectMany(Func<TIn, IEnumerable<TOut>> parameter)
				: base((body, stores, func, limit) => new Query.SelectMany<TIn, TOut>(body, func, stores, limit),
				       parameter) {}
		}

		public class InlineSelect<TIn, TOut> : Builder<TIn, TOut, Expression<Func<TIn, TOut>>>
		{
			public InlineSelect(Expression<Func<TIn, TOut>> parameter)
				: base((body, stores, expression, limit)
					       => new InlineProjection<TIn, TOut>(body, expression, stores, limit),
				       parameter) {}
		}

		public sealed class Select<TIn, TOut> : Builder<TIn, TOut, Func<TIn, TOut>>
		{
			public Select(Func<TIn, TOut> argument)
				: base((shape, stores, parameter, limit) => new Query.Select<TIn, TOut>(shape, stores, parameter, limit),
				       argument) {}
		}

		public sealed class Where<T> : BodyBuilder<T, Func<T, bool>>
		{
			public Where(Func<T, bool> where)
				: base((parameter, selection, limit) => new Query.Where<T>(parameter, selection, limit), where) {}
		}

		public sealed class Distinct<T> : BodyBuilder<T, IEqualityComparer<T>>
		{
			public static Distinct<T> Default { get; } = new Distinct<T>();

			Distinct() : this(EqualityComparer<T>.Default) {}

			public Distinct(IEqualityComparer<T> comparer)
				: base((parameter, selection, limit) => new Query.Distinct<T>(parameter), comparer) {}
		}
	}
}