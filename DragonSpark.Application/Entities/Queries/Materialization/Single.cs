namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class Single<TIn, T> : Materialize<TIn, T, T>
	{
		public Single(IQuery<TIn, T> query) : this(query, SingleMaterializer<T>.Default) {}

		protected Single(IQuery<TIn, T> query, IMaterializer<T, T> materializer) : base(query, materializer) {}
	}
}