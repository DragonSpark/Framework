using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results
{
	public class Validated<T> : IResult<T>
	{
		readonly Func<T> _source, _fallback;

		readonly Func<bool> _specification;

		public Validated(ICondition specification, IResult<T> result, IResult<T> fallback)
			: this(specification.Get, result.Get, fallback.Get) {}

		public Validated(Func<bool> specification, Func<T> source, Func<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public T Get()
		{
			var source = _specification() ? _source : _fallback;
			var result = source();
			return result;
		}

		public static implicit operator T(Validated<T> source) => source.Get();
	}
}