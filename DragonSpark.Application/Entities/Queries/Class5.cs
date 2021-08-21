namespace DragonSpark.Application.Entities.Queries
{
	class Class5 {}

	/*public class Update<TIn, T> : ISelecting<TIn, T> where T : class
	{
		readonly IQuery<TIn, T>      _query;
		readonly Action<T>           _update;
		readonly IMaterializer<T, T> _materializer;

		protected Update(IQuery<TIn, T> query, Action<T> update) : this(query, update, SingleMaterializer<T>.Default) {}

		protected Update(IQuery<TIn, T> query, Action<T> update, IMaterializer<T, T> materializer)
		{
			_query        = query;
			_update       = update;
			_materializer = materializer;
		}

		public async ValueTask<T> Get(TIn parameter)
		{
			await using var session = _query.Get(parameter);
			var (context, subject) = session;
			var result = await _materializer.Await(subject);
			_update(result);
			await context.SaveChangesAsync().ConfigureAwait(false);
			return result;
		}
	}*/
}