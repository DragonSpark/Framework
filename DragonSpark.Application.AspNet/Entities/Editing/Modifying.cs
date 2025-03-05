using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Editing;

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
		using var edit = await _select.Get(parameter).Go();
		await _configure(edit);
		await edit.Await();
		return edit.Subject;
	}
}
