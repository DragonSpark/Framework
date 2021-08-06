using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class Materialize<TIn, TEntity, TResult> : ISelecting<TIn, TResult>
	{
		readonly IQuery<TIn, TEntity>            _query;
		readonly IMaterializer<TEntity, TResult> _materializer;

		protected Materialize(IQuery<TIn, TEntity> query, IMaterializer<TEntity, TResult> materializer)
		{
			_query        = query;
			_materializer = materializer;
		}

		public ValueTask<TResult> Get(TIn parameter) => _materializer.Get(_query.Get(parameter));
	}
}