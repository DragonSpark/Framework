using System;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Results
{
	public class ValidatedResult<T> : IResult<T>
	{
		readonly Func<T> _source, _fallback;

		readonly Func<T, bool> _specification;

		public ValidatedResult(IResult<T> result, IResult<T> fallback) :
			this(IsAssigned<T>.Default, result, fallback) {}

		public ValidatedResult(ICondition<T> specification, IResult<T> result, IResult<T> fallback)
			: this(specification.Get, result.Get, fallback.Get) {}

		public ValidatedResult(Func<T, bool> specification, Func<T> source, Func<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public T Get()
		{
			var source = _source();
			var result = _specification(source) ? source : _fallback();
			return result;
		}

		public static implicit operator T(ValidatedResult<T> source) => source.Get();
	}
}