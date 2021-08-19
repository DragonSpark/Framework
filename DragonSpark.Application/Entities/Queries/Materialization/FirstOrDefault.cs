namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class FirstOrDefault<TIn, T> : Materialize<TIn, T, T?>
	{
		public FirstOrDefault(Scoped.IQuery<TIn, T> query) : this(query, FirstOrDefaultMaterializer<T>.Default) {}

		protected FirstOrDefault(Scoped.IQuery<TIn, T> query, IMaterializer<T, T?> materializer)
			: base(query, materializer) {}
	}
}