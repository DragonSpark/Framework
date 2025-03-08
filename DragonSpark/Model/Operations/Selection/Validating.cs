using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class Validating<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly Await<TIn, bool> _specification;
	readonly Await<TIn, TOut> _true, _false;

	public Validating(IDepending<TIn> condition, ISelecting<TIn, TOut> @true)
		: this(condition, @true, Default<TIn, TOut>.Instance.Then().Operation().Out()) {}

	public Validating(IDepending<TIn> condition, ISelecting<TIn, TOut> @true, ISelecting<TIn, TOut> @false)
		: this(condition.Off, @true.Off, @false.Off) {}

	public Validating(Await<TIn, bool> specification, Await<TIn, TOut> @true)
		: this(specification, @true, Default<TIn, TOut>.Instance.Then().Operation()) {}

	public Validating(Await<TIn, bool> specification, Await<TIn, TOut> @true, Await<TIn, TOut> @false)
	{
		_specification = specification;
		_true          = @true;
		_false         = @false;
	}

	public async ValueTask<TOut> Get(TIn parameter)
		=> await _specification(parameter) ? await _true(parameter) : await _false(parameter);
}