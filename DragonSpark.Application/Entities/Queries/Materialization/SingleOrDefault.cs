namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class SingleOrDefault<TIn, T> : Materialize<TIn, T, T?>
	{
		public SingleOrDefault(Scoped.IQuery<TIn, T> query) : this(query, SingleOrDefaultMaterializer<T>.Default) {}

		protected SingleOrDefault(Scoped.IQuery<TIn, T> query, IMaterializer<T, T?> materializer)
			: base(query, materializer) {}
	}
}