using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class Modifying<TIn, T> : ISelecting<TIn, T>
{
	readonly IEdit<TIn, T>  _select;
	readonly Await<Edit<T>> _configure;

	protected Modifying(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Await) {}

	protected Modifying(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

	protected Modifying(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Await) {}

	protected Modifying(IEdit<TIn, T> select, Await<Edit<T>> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask<T> Get(TIn parameter)
	{
		using var edit = await _select.Get(parameter);
		await _configure(edit);
		await edit.Await();
		return edit.Subject;
	}
}