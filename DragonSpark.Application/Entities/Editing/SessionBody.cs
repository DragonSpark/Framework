using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public class SessionBody<TIn, TOut, TSave> : ISessionBody<TIn, TOut, TSave>
	{
		readonly ISelecting<TIn, TOut?> _select;
		readonly IOperation<TSave>      _save;

		protected SessionBody(ISelecting<TIn, TOut?> select, IOperation<TSave> save)
		{
			_select = select;
			_save   = save;
		}

		public ValueTask<TOut?> Get(TIn parameter) => _select.Get(parameter);

		public ValueTask Get(TSave parameter) => _save.Get(parameter);
	}
}