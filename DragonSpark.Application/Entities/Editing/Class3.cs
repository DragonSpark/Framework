using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	class Class3 {}

	public interface ISession<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave> {}

	public interface ISessionBody<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave> {}

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

	public class Session<TIn, TOut, TSave> : ISession<TIn, TOut, TSave>
	{
		readonly ISelecting<TIn, TOut?> _select;
		readonly IOperation<TSave>      _apply;

		protected Session(ISessionScopes scopes, IQuery<TIn, TOut> select, IOperation<TSave> apply)
			: this(scopes.Then().Use(select).To.SingleOrDefault(), apply) {}

		protected Session(ISelecting<TIn, TOut?> select, IOperation<TSave> apply)
		{
			_select  = select;
			_apply   = apply;
		}

		public ValueTask<TOut?> Get(TIn parameter) => _select.Get(parameter);

		public ValueTask Get(TSave parameter) => _apply.Get(parameter);
	}

	/**/
}