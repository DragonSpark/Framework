using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Validating<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Await<TIn, TOut> _source, _fallback;
		readonly Await<TIn, bool> _specification;

		public Validating(IDepending<TIn> condition, ISelecting<TIn, TOut> select)
			: this(condition, select, Default<TIn, TOut>.Instance.Then().Operation().Out()) {}

		public Validating(IDepending<TIn> condition, ISelecting<TIn, TOut> select, ISelecting<TIn, TOut> fallback)
			: this(condition.Await, select.Await, fallback.Await) {}

		public Validating(Await<TIn, bool> specification, Await<TIn, TOut> source)
			: this(specification, source, Default<TIn, TOut>.Instance.Then().Operation()) {}

		public Validating(Await<TIn, bool> specification, Await<TIn, TOut> source, Await<TIn, TOut> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public async ValueTask<TOut> Get(TIn parameter)
			=> await _specification(parameter) ? await _source(parameter) : await _fallback(parameter);
	}

	public class Validating<T> : IOperation<T>
	{
		readonly Await<T>       _source, _fallback;
		readonly Await<T, bool> _specification;

		public Validating(IDepending<T> condition, IOperation<T> select)
			: this(condition.Await, select.Await, Default<T>.Instance.Then().Operation()) {}

		public Validating(IDepending<T> condition, IOperation<T> select, IOperation<T> fallback)
			: this(condition.Await, select.Await, fallback.Await) {}

		public Validating(Await<T, bool> specification, Await<T> source)
			: this(specification, source, Default<T>.Instance.Then().Operation()) {}

		public Validating(Await<T, bool> specification, Await<T> source, Await<T> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public async ValueTask Get(T parameter)
		{
			var operation = await _specification(parameter) ? _source : _fallback;
			await operation(parameter);
		}
	}

}