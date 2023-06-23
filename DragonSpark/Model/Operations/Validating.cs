using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Validating : IOperation
{
	readonly Await<None, bool> _specification;
	readonly Await             _true, _false;

	public Validating(IDepending<None> condition, IOperation @true)
		: this(condition.Await, @true.Await, EmptyOperation.Default.Await) {}

	public Validating(IDepending<None> condition, IOperation @true, IOperation @false)
		: this(condition.Await, @true.Await, @false.Await) {}

	public Validating(Await<None, bool> specification, Await @true)
		: this(specification, @true, EmptyOperation.Default.Await) {}

	public Validating(Await<None, bool> specification, Await @true, Await @false)
	{
		_specification = specification;
		_true          = @true;
		_false         = @false;
	}

	public async ValueTask Get()
	{
		var operation = await _specification(None.Default) ? _true : _false;
		await operation();
	}

}

public class Validating<T> : IOperation<T>
{
	readonly Await<T, bool> _specification;
	readonly Await<T>       _true, _false;

	public Validating(IDepending<T> condition, IOperation<T> @true)
		: this(condition.Await, @true.Await, Default<T>.Instance.Then().Operation()) {}

	public Validating(IDepending<T> condition, IOperation<T> @true, IOperation<T> @false)
		: this(condition.Await, @true.Await, @false.Await) {}

	public Validating(Await<T, bool> specification, Await<T> @true)
		: this(specification, @true, Default<T>.Instance.Then().Operation()) {}

	public Validating(Await<T, bool> specification, Await<T> @true, Await<T> @false)
	{
		_specification = specification;
		_true          = @true;
		_false         = @false;
	}

	public async ValueTask Get(T parameter)
	{
		var operation = await _specification(parameter) ? _true : _false;
		await operation(parameter);
	}
}