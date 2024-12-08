using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Session<TIn, TOut, TSave> : ISession<TIn, TOut, TSave>
{
	readonly ISelecting<TIn, TOut?> _select;
	readonly IOperation<TSave>      _apply;

	protected Session(ISelecting<TIn, TOut?> select, IOperation<TSave> apply)
	{
		_select = select;
		_apply  = apply;
	}

	public ValueTask<TOut?> Get(TIn parameter) => _select.Get(parameter);

	public ValueTask Get(TSave parameter) => _apply.Get(parameter);
}