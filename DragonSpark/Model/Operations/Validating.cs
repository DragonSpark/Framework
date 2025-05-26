using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Validating<T> : IOperation<T>
{
	readonly Await<T, bool> _specification;
	readonly Await<T>       _true, _false;

	public Validating(ISelect<T, ValueTask<bool>> condition, IOperation<T> @true)
		: this(condition.Off, @true.Off, Default<T>.Instance.Then().Operation()) {}

	public Validating(ISelect<T, ValueTask<bool>> condition, IOperation<T> @true, IOperation<T> @false)
		: this(condition.Off, @true.Off, @false.Off) {}

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